using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.CourseLearningOutcome;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseLearningOutcomeController : ControllerBase
    {
        private readonly ICourseLearningOutcomeService _courseLearningOutcomeService;

        public CourseLearningOutcomeController(ICourseLearningOutcomeService courseLearningOutcomeService)
        {
            _courseLearningOutcomeService = courseLearningOutcomeService;
        }

        [HttpGet("get-clo-by-id/{id}")]
        public async Task<IActionResult> GetCourseLearningOutcomeById(Guid id)
        {
            var response = await _courseLearningOutcomeService.GetCourseLearningOutcomeById(id);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-all-clos")]
        public async Task<IActionResult> GetAllCourseLearningOutcomes(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = "",
            [FromQuery] SortBy sortBy = SortBy.Default,
            [FromQuery] SortType sortType = SortType.Ascending,
            [FromQuery] bool isDelete = false)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            var response = await _courseLearningOutcomeService.GetAllCourseLearningOutcomes(
                pageNumber, pageSize, search, sortBy, sortType, isDelete);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-clo")]
        public async Task<IActionResult> CreateCourseLearningOutcome([FromBody] CreateCourseLearningOutcomeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _courseLearningOutcomeService.CreateCourseLearningOutcome(request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("update-curriculum/{cloId}")]
        public async Task<IActionResult> UpdateCourseLearningOutcome(
            Guid cloId,
            UpdateCourseLearningOutcomeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _courseLearningOutcomeService.UpdateCourseLearningOutcome(cloId, request);

            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-curriculum/{cloId}")]
        public async Task<IActionResult> DeleteCourseLearningOutcome(Guid cloId)
        {
            var response = await _courseLearningOutcomeService.DeleteCourseLearningOutcome(cloId);

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
