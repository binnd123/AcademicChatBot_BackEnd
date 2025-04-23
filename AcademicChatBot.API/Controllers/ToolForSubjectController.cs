using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.Tools;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/tool-for-subject")]
    [ApiController]
    public class ToolForSubjectController : ControllerBase
    {
        private readonly IToolForSubjectService _toolForSubjectService;

        public ToolForSubjectController(IToolForSubjectService toolForSubjectService)
        {
            _toolForSubjectService = toolForSubjectService;
        }

        [HttpGet("get-tool-for-subject-by-id/{toolForSubjectId}")]
        public async Task<IActionResult> GetToolForSubjectById(Guid toolForSubjectId)
        {
            var response = await _toolForSubjectService.GetToolForSubjectById(toolForSubjectId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-all-tools-for-subject")]
        public async Task<IActionResult> GetAllToolsForSubject(
            [FromQuery] Guid subjectId, 
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            var response = await _toolForSubjectService.GetAllToolsForSubject(subjectId, pageNumber, pageSize);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-all-subjects-for-tool")]
        public async Task<IActionResult> GetAllSubjectsForTool(
            [FromQuery] Guid toolId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            var response = await _toolForSubjectService.GetAllSubjectsForTool(toolId, pageNumber, pageSize);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add-subjects-to-tool")]
        public async Task<IActionResult> AddSubjectsToTool(
            [FromQuery] Guid toolId, 
            [FromBody] List<Guid> subjectIds)
        {
            var response = await _toolForSubjectService.AddSubjectsToTool(toolId, subjectIds);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add-tools-to-subject")]
        public async Task<IActionResult> AddToolsToSubject(
            [FromQuery] Guid subjectId,
            [FromBody] List<Guid> toolIds)
        {
            var response = await _toolForSubjectService.AddToolsToSubject(subjectId, toolIds);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-tools-from-subject")]
        public async Task<IActionResult> DeleteToolsFromSubject(
            [FromQuery] Guid subjectId,
            [FromBody] List<Guid> toolIds)
        {
            var response = await _toolForSubjectService.DeleteToolsFromSubject(subjectId, toolIds);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-subjects-from-tool")]
        public async Task<IActionResult> DeleteSubjectsFromTool(
            [FromQuery] Guid toolId,
            [FromBody] List<Guid> subjectIds)
        {
            var response = await _toolForSubjectService.DeleteSubjectsFromTool(toolId, subjectIds);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-all-tools-from-subject")]
        public async Task<IActionResult> DeleteAllToolsFromSubject(
            [FromQuery] Guid subjectId)
        {
            var response = await _toolForSubjectService.DeleteAllToolsFromSubject(subjectId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-all-subjects-from-tool")]
        public async Task<IActionResult> DeleteAllSubjectsFromTool(
            [FromQuery] Guid toolId)
        {
            var response = await _toolForSubjectService.DeleteAllSubjectsFromTool(toolId);
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