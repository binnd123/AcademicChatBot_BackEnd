using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.ComboSubject;
using AcademicChatBot.Common.BussinessModel.SubjectInCurriculum;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/combo-subjects")]
    [ApiController]
    public class ComboSubjectController : ControllerBase
    {
        private readonly IComboSubjectService _comboSubjectService;

        public ComboSubjectController(IComboSubjectService comboSubjectService)
        {
            _comboSubjectService = comboSubjectService;
        }

        //[HttpGet("{comboSubjectId}")]
        //public async Task<IActionResult> GetComboSubjectById(Guid comboSubjectId)
        //{
        //    var response = await _comboSubjectService.GetComboSubjectById(comboSubjectId);
        //    if (response.IsSucess == false)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        //[HttpGet("subject/{subjectId}/combos")]
        //public async Task<IActionResult> GetAllCombosForSubject(
        //    Guid subjectId,
        //    [FromQuery] int pageNumber = 1,
        //    [FromQuery] int pageSize = 5
        //)
        //{
        //    pageNumber = pageNumber < 1 ? 1 : pageNumber;
        //    pageSize = pageSize < 1 ? 5 : pageSize;
        //    var response = await _comboSubjectService.GetAllCombosForSubject(subjectId, pageNumber, pageSize);
        //    if (response.IsSucess == false)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        [HttpGet("combo/{comboId}/subjects")]
        public async Task<IActionResult> GetAllSubjectsForCombo(
            Guid comboId,
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

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //public async Task<IActionResult> AddComboSubject(
        //    [FromQuery] Guid comboId,
        //    [FromQuery] Guid subjectIds,
        //    [FromQuery] int semesterNo,
        //    [FromQuery] string? note)
        //{
        //    var response = await _comboSubjectService.AddComboSubject(comboId, subjectIds, semesterNo, note);
        //    if (response.IsSucess == false)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddSubjectsToCombo(
            [FromQuery] Guid comboId,
            [FromBody] List<SubjectInComboRequest> requests)
        {
            var response = await _comboSubjectService.AddSubjectsToCombo(comboId, requests);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return BadRequest(response);
            }
            return Ok(response);
        }

        //[Authorize(Roles = "Admin")]
        //[HttpDelete("subject/{subjectId}/combos")]
        //public async Task<IActionResult> DeleteCombosFromSubject(
        //    Guid subjectId,
        //    [FromBody] List<Guid> comboIds)
        //{
        //    var response = await _comboSubjectService.DeleteCombosFromSubject(subjectId, comboIds);
        //    if (response.IsSucess == false)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        [Authorize(Roles = "Admin")]
        [HttpDelete("combo/{comboId}/subjects")]
        public async Task<IActionResult> DeleteSubjectsFromCombo(
            Guid comboId,
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

        //[Authorize(Roles = "Admin")]
        //[HttpDelete("subject/{subjectId}/combos/all")]
        //public async Task<IActionResult> DeleteAllCombosFromSubject(
        //    Guid subjectId)
        //{
        //    var response = await _comboSubjectService.DeleteAllCombosFromSubject(subjectId);
        //    if (response.IsSucess == false)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        [Authorize(Roles = "Admin")]
        [HttpDelete("combo/{comboId}/subjects/all")]
        public async Task<IActionResult> DeleteAllSubjectsFromCombo(
            Guid comboId)
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
