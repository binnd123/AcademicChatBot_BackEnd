using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.PrerequisiteSubject;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrerequisiteSubjectController : ControllerBase
    {
        private readonly IPrerequisiteSubjectService _prerequisiteSubjectService;

        public PrerequisiteSubjectController(IPrerequisiteSubjectService prerequisiteSubjectService)
        {
            _prerequisiteSubjectService = prerequisiteSubjectService;
        }

        [Authorize]
        [HttpGet("get-all-prerequisite-subjects")]
        public async Task<IActionResult> GetAllPrerequisiteSubjects(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] SortBy sortBy = SortBy.Default,
            [FromQuery] SortType sortType = SortType.Ascending,
            [FromQuery] bool isDelete = false)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            var response = await _prerequisiteSubjectService.GetAllPrerequisiteSubjects(pageNumber, pageSize, sortBy, sortType, isDelete);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("get-prerequisite-subject-by-id/{id}")]
        public async Task<IActionResult> GetPrerequisiteSubjectById(Guid id)
        {
            var response = await _prerequisiteSubjectService.GetPrerequisiteSubjectById(id);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-prerequisite-subject")]
        public async Task<IActionResult> CreatePrerequisiteSubject([FromBody] CreatePrerequisiteSubjectRequest request)
        {
            var response = await _prerequisiteSubjectService.CreatePrerequisiteSubject(request);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-prerequisite-subject/{id}")]
        public async Task<IActionResult> UpdatePrerequisiteSubject(
            Guid id,
            UpdatePrerequisiteSubjectRequest request)
        {
            var response = await _prerequisiteSubjectService.UpdatePrerequisiteSubject(id, request);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-prerequisite-subject/{id}")]
        public async Task<IActionResult> DeletePrerequisiteSubject(Guid id)
        {
            var response = await _prerequisiteSubjectService.DeletePrerequisiteSubject(id);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
