using AcademicChatBot.Common.BussinessModel.ProgramingOutcome;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Implementation;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/programing-outcomes")]
    [ApiController]
    public class ProgramingOutcomeController : ControllerBase
    {
        private readonly IProgramingOutcomeService _programingOutcomeService;

        public ProgramingOutcomeController(IProgramingOutcomeService programingOutcomeService)
        {
            _programingOutcomeService = programingOutcomeService;
        }

        // Lấy thông tin outcome theo ID
        [HttpGet("{pOId}")]
        public async Task<IActionResult> GetProgramingOutcomeById(Guid pOId)
        {
            var response = await _programingOutcomeService.GetProgramingOutcomeById(pOId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Lấy tất cả outcomes
        [HttpGet]
        public async Task<IActionResult> GetAllProgramingOutcomes(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = "",
            [FromQuery] SortBy sortBy = SortBy.Default,
            [FromQuery] SortType sortType = SortType.Ascending,
            [FromQuery] bool isDeleted = false
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            var response = await _programingOutcomeService.GetAllProgramingOutcomes(pageNumber, pageSize, search, sortBy, sortType, isDeleted);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Tạo outcome mới (chỉ dành cho admin)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProgramingOutcome([FromBody] CreateProgramingOutcomeRequest request)
        {
            var response = await _programingOutcomeService.CreateProgramingOutcome(request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Cập nhật outcome (chỉ dành cho admin)
        [Authorize(Roles = "Admin")]
        [HttpPut("{pOId}")]
        public async Task<IActionResult> UpdateProgramingOutcome(Guid pOId, UpdateProgramingOutcomeRequest request)
        {
            var response = await _programingOutcomeService.UpdateProgramingOutcome(pOId, request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Xóa outcome (chỉ dành cho admin)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{pOId}")]
        public async Task<IActionResult> DeleteProgramingOutcome(Guid pOId)
        {
            var response = await _programingOutcomeService.DeleteProgramingOutcome(pOId);
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
