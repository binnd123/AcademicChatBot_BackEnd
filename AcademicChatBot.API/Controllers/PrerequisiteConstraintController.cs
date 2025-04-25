using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.PrerequisiteConstraint;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrerequisiteConstraintController : ControllerBase
    {
        private readonly IPrerequisiteConstraintService _prerequisiteConstraintService;

        public PrerequisiteConstraintController(IPrerequisiteConstraintService prerequisiteConstraintService)
        {
            _prerequisiteConstraintService = prerequisiteConstraintService;
        }

        [HttpGet("get-all-prerequisite-constraints")]
        public async Task<IActionResult> GetAllPrerequisiteConstraints(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = "",
            [FromQuery] SortBy sortBy = SortBy.Default,
            [FromQuery] SortType sortType = SortType.Ascending,
            [FromQuery] bool isDelete = false)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            var response = await _prerequisiteConstraintService.GetAllPrerequisiteConstraints(pageNumber, pageSize, search, sortBy, sortType, isDelete);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-prerequisite-constraint-by-id/{id}")]
        public async Task<IActionResult> GetPrerequisiteConstraintById(Guid id)
        {
            var response = await _prerequisiteConstraintService.GetPrerequisiteConstraintById(id);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-prerequisite-constraint")]
        public async Task<IActionResult> CreatePrerequisiteConstraint([FromBody] CreatePrerequisiteConstraintRequest request)
        {
            var response = await _prerequisiteConstraintService.CreatePrerequisiteConstraint(request);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-prerequisite-constraint/{id}")]
        public async Task<IActionResult> UpdatePrerequisiteConstraint(
            Guid id,
            UpdatePrerequisiteConstraintRequest request)
        {
            var response = await _prerequisiteConstraintService.UpdatePrerequisiteConstraint(id, request);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-prerequisite-constraint/{id}")]
        public async Task<IActionResult> DeletePrerequisiteConstraint(Guid id)
        {
            var response = await _prerequisiteConstraintService.DeletePrerequisiteConstraint(id);
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
