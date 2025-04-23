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
    [Route("api/programing-learning-outcome")]
    [ApiController]
    public class ProgramingLearningOutcomeController : ControllerBase
    {
        private readonly IProgramingLearningOutcomeService _programingLearningOutcomeService;

        public ProgramingLearningOutcomeController(IProgramingLearningOutcomeService programingLearningOutcomeService)
        {
            _programingLearningOutcomeService = programingLearningOutcomeService;
        }

        [Authorize]
        [HttpGet("get-programing-learning-outcome-by-id/{pLOId}")]
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

        [Authorize]
        [HttpGet("get-all-programing-learning-outcomes")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost("create-programing-learning-outcome")]
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

        [Authorize(Roles = "Admin")]
        [HttpPut("update-programing-learning-outcome/{pLOId}")]
        public async Task<IActionResult> UpdateProgramingLearningOutcome(Guid pLOId, [FromBody] UpdateProgramingLearningOutcomeRequest request)
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-programing-learning-outcome/{pLOId}")]
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
