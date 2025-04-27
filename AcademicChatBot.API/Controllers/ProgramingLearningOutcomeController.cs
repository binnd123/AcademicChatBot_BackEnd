using AcademicChatBot.Common.BussinessModel.ProgramingLearningOutcome;
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
    [Route("api/programing-learning-outcomes")]
    [ApiController]
    public class ProgramingLearningOutcomeController : ControllerBase
    {
        private readonly IProgramingLearningOutcomeService _programingLearningOutcomeService;

        public ProgramingLearningOutcomeController(IProgramingLearningOutcomeService programingLearningOutcomeService)
        {
            _programingLearningOutcomeService = programingLearningOutcomeService;
        }

        // Lấy thông tin outcome học tập theo ID
        [HttpGet("{pLOId}")]
        public async Task<IActionResult> GetProgramingLearningOutcomeById(Guid pLOId)
        {
            var response = await _programingLearningOutcomeService.GetProgramingLearningOutcomeById(pLOId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Lấy tất cả các outcome học tập
        [HttpGet]
        public async Task<IActionResult> GetAllProgramingLearningOutcomes(
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
            var response = await _programingLearningOutcomeService.GetAllProgramingLearningOutcomes(pageNumber, pageSize, search, sortBy, sortType, isDelete);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Tạo outcome học tập mới (chỉ dành cho admin)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProgramingLearningOutcome([FromBody] CreateProgramingLearningOutcomeRequest request)
        {
            var response = await _programingLearningOutcomeService.CreateProgramingLearningOutcome(request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Cập nhật outcome học tập (chỉ dành cho admin)
        [Authorize(Roles = "Admin")]
        [HttpPut("{pLOId}")]
        public async Task<IActionResult> UpdateProgramingLearningOutcome(Guid pLOId, UpdateProgramingLearningOutcomeRequest request)
        {
            var response = await _programingLearningOutcomeService.UpdateProgramingLearningOutcome(pLOId, request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Xóa outcome học tập (chỉ dành cho admin)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{pLOId}")]
        public async Task<IActionResult> DeleteProgramingLearningOutcome(Guid pLOId)
        {
            var response = await _programingLearningOutcomeService.DeleteProgramingLearningOutcome(pLOId);
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
