using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.Programs;
using AcademicChatBot.Common.BussinessModel.Tools;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/programs")]
    [ApiController]
    public class ProgramController : ControllerBase
    {
        private readonly IProgramService _programService;

        public ProgramController(IProgramService programService)
        {
            _programService = programService;
        }

        // Lấy thông tin chương trình học theo ID
        [HttpGet("{programId}")]
        public async Task<IActionResult> GetProgramById(Guid programId)
        {
            var response = await _programService.GetProgramById(programId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Lấy tất cả các chương trình học
        [HttpGet]
        public async Task<IActionResult> GetAllPrograms(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = "",
            [FromQuery] SortBy sortBy = SortBy.Default,
            [FromQuery] SortType sortType = SortType.Ascending,
            [FromQuery] bool isDelete = false
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            var response = await _programService.GetAllPrograms(pageNumber, pageSize, search, sortBy, sortType, isDelete);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Tạo chương trình học mới (chỉ dành cho admin)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProgram([FromBody] CreateProgramRequest request)
        {
            var response = await _programService.CreateProgram(request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Cập nhật chương trình học (chỉ dành cho admin)
        [Authorize(Roles = "Admin")]
        [HttpPut("{programId}")]
        public async Task<IActionResult> UpdateProgram(Guid programId, [FromBody] UpdateProgramRequest request)
        {
            var response = await _programService.UpdateProgram(programId, request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Xóa chương trình học (chỉ dành cho admin)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{programId}")]
        public async Task<IActionResult> DeleteProgram(Guid programId)
        {
            var response = await _programService.DeleteProgram(programId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }
    }

}
