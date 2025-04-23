using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/combo-subject")]
    [ApiController]
    public class ComboSubjectController : ControllerBase
    {
        private readonly IComboSubjectService _comboSubjectService;

        public ComboSubjectController(IComboSubjectService comboSubjectService)
        {
            _comboSubjectService = comboSubjectService;
        }

        [HttpGet("get-combo-subject-by-id/{comboSubjectId}")]
        public async Task<IActionResult> GetComboSubjectById(Guid comboSubjectId)
        {
            var response = await _comboSubjectService.GetComboSubjectById(comboSubjectId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-all-combos-for-subject")]
        public async Task<IActionResult> GetAllCombosForSubject(
            [FromQuery] Guid subjectId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            var response = await _comboSubjectService.GetAllCombosForSubject(subjectId, pageNumber, pageSize);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-all-subjects-for-combo")]
        public async Task<IActionResult> GetAllSubjectsForCombo(
            [FromQuery] Guid comboId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            var response = await _comboSubjectService.GetAllSubjectsForCombo(comboId, pageNumber, pageSize);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add-combo-subject")]
        public async Task<IActionResult> AddSubjectsToTool(
        [FromQuery] Guid comboId,
        [FromQuery] Guid subjectIds,
        [FromQuery] int semesterNo,
        [FromQuery] string? note)
        {
            var response = await _comboSubjectService.AddComboSubject(comboId, subjectIds, semesterNo, note);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-combos-from-subject")]
        public async Task<IActionResult> DeleteCombosFromSubject(
        [FromQuery] Guid subjectId,
        [FromBody] List<Guid> comboIds)
        {
            var response = await _comboSubjectService.DeleteCombosFromSubject(subjectId, comboIds);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-subjects-from-combo")]
        public async Task<IActionResult> DeleteSubjectsFromCombo(
        [FromQuery] Guid comboId,
        [FromBody] List<Guid> subjectIds)
        {
            var response = await _comboSubjectService.DeleteSubjectsFromCombo(comboId, subjectIds);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-all-combos-from-subject")]
        public async Task<IActionResult> DeleteAllCombosFromSubject(
            [FromQuery] Guid subjectId)
        {
            var response = await _comboSubjectService.DeleteAllCombosFromSubject(subjectId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-all-subjects-from-combo")]
        public async Task<IActionResult> DeleteAllSubjectsFromCombo(
            [FromQuery] Guid comboId)
        {
            var response = await _comboSubjectService.DeleteAllSubjectsFromCombo(comboId);
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
