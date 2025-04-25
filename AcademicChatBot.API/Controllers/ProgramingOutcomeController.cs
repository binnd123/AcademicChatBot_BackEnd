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
    [Route("api/programing-outcome")]
    [ApiController]
    public class ProgramingOutcomeController : ControllerBase
    {
        private readonly IProgramingOutcomeService _programingOutcomeService;

        public ProgramingOutcomeController(IProgramingOutcomeService programingOutcomeService)
        {
            _programingOutcomeService = programingOutcomeService;
        }

        [HttpGet("get-programing-outcome-by-id/{pOId}")]
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

        [HttpGet("get-all-programing-outcomes")]
        public async Task<IActionResult> GetAllProgramingOutcomes(
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
            var response = await _programingOutcomeService.GetAllProgramingOutcomes(pageNumber, pageSize, search, sortBy, sortType, isDelete);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-programing-outcome")]
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

        [Authorize(Roles = "Admin")]
        [HttpPut("update-programing-outcome/{pOId}")]
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

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-programing-outcome/{pOId}")]
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
