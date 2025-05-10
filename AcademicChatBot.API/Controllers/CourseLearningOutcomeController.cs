using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.CourseLearningOutcome;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/course-learning-outcomes")]
    [ApiController]
    public class CourseLearningOutcomeController : ControllerBase
    {
        private readonly ICourseLearningOutcomeService _courseLearningOutcomeService;

        public CourseLearningOutcomeController(ICourseLearningOutcomeService courseLearningOutcomeService)
        {
            _courseLearningOutcomeService = courseLearningOutcomeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
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

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = "",
            [FromQuery] SortBy sortBy = SortBy.Default,
            [FromQuery] SortType sortType = SortType.Ascending,
            [FromQuery] bool isDeleted = false)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            var response = await _courseLearningOutcomeService.GetAllCourseLearningOutcomes(
                pageNumber, pageSize, search, sortBy, sortType, isDeleted);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseLearningOutcomeRequest request)
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

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateCourseLearningOutcomeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _courseLearningOutcomeService.UpdateCourseLearningOutcome(id, request);

            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _courseLearningOutcomeService.DeleteCourseLearningOutcome(id);

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
