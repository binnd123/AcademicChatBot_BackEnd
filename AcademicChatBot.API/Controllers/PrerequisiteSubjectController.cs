using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.PrerequisiteSubject;
using AcademicChatBot.Common.BussinessModel.SubjectInCurriculum;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/prerequisite-subject")]
    [ApiController]
    public class PrerequisiteSubjectController : ControllerBase
    {
        private readonly IPrerequisiteSubjectService _prerequisiteSubjectService;

        public PrerequisiteSubjectController(IPrerequisiteSubjectService prerequisiteSubjectService)
        {
            _prerequisiteSubjectService = prerequisiteSubjectService;
        }

        //[HttpGet("get-all-prerequisite-subjects")]
        //public async Task<IActionResult> GetAllPrerequisiteSubjects(
        //    [FromQuery] int pageNumber = 1,
        //    [FromQuery] int pageSize = 5,
        //    [FromQuery] SortBy sortBy = SortBy.Default,
        //    [FromQuery] SortType sortType = SortType.Ascending,
        //    [FromQuery] bool isDelete = false)
        //{
        //    pageNumber = pageNumber < 1 ? 1 : pageNumber;
        //    pageSize = pageSize < 1 ? 5 : pageSize;
        //    var response = await _prerequisiteSubjectService.GetAllPrerequisiteSubjects(pageNumber, pageSize, sortBy, sortType, isDelete);
        //    if (!response.IsSucess)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPost("create-prerequisite-subject")]
        //public async Task<IActionResult> CreatePrerequisiteSubject([FromBody] CreatePrerequisiteSubjectRequest request)
        //{
        //    var response = await _prerequisiteSubjectService.CreatePrerequisiteSubject(request);
        //    if (!response.IsSucess)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPut("update-prerequisite-subject/{id}")]
        //public async Task<IActionResult> UpdatePrerequisiteSubject(
        //    Guid id,
        //    UpdatePrerequisiteSubjectRequest request)
        //{
        //    var response = await _prerequisiteSubjectService.UpdatePrerequisiteSubject(id, request);
        //    if (!response.IsSucess)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpDelete("delete-prerequisite-subject/{id}")]
        //public async Task<IActionResult> DeletePrerequisiteSubject(Guid id)
        //{
        //    var response = await _prerequisiteSubjectService.DeletePrerequisiteSubject(id);
        //    if (!response.IsSucess)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}


        //[HttpGet("get-prerequisite-subject-by-id/{id}")]
        //public async Task<IActionResult> GetPrerequisiteSubjectById(Guid id)
        //{
        //    var response = await _prerequisiteSubjectService.GetPrerequisiteSubjectById(id);
        //    if (!response.IsSucess)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        [HttpGet("get-all-prerequisite-expression-for-constrain")]
        public async Task<IActionResult> GetReadablePrerequisiteExpression(
            [FromQuery] Guid prerequisiteConstrainId)
        {
            var response = await _prerequisiteSubjectService.GetReadablePrerequisiteExpression(prerequisiteConstrainId);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-all-prerequisite-expression-of-subject-in-curriculum")]
        public async Task<IActionResult> GetReadablePrerequisiteExpressionOfSubjectInCurriculum(
            [FromQuery] Guid subjectId,
            [FromQuery] Guid curriculumId)
        {
            var response = await _prerequisiteSubjectService.GetReadablePrerequisiteExpressionOfSubjectInCurriculum(subjectId, curriculumId);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        //[HttpGet("get-all-prerequisite-subjects-for-prerequisite-constrain")]
        //public async Task<IActionResult> GetAllPrerequisiteSubjectsForPrerequisiteConstrain(
        //    [FromQuery] Guid prerequisiteConstrainId,
        //    [FromQuery] int pageNumber = 1,
        //    [FromQuery] int pageSize = 5)
        //{
        //    pageNumber = pageNumber < 1 ? 1 : pageNumber;
        //    pageSize = pageSize < 1 ? 5 : pageSize;
        //    var response = await _prerequisiteSubjectService.GetAllPrerequisiteSubjectsForPrerequisiteConstrain(prerequisiteConstrainId, pageNumber, pageSize);
        //    if (!response.IsSucess)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        [Authorize(Roles = "Admin")]
        [HttpPost("add-prerequisite-subjects-to-prerequisite-constrain")]
        public async Task<IActionResult> AddPrerequisiteSubjectsToPrerequisiteConstrain(
            [FromQuery] Guid prerequisiteConstrainId,
            [FromBody] List<PrerequisiteSubjectsToPrerequisiteConstrainRequest> requests)
        {
            var response = await _prerequisiteSubjectService.AddPrerequisiteSubjectsToPrerequisiteConstrain(prerequisiteConstrainId, requests);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return BadRequest(response);
            }
            return Ok(response);
        }

        //[Authorize(Roles = "Admin")]
        //[HttpDelete("delete-prerequisite-subjects-from-prerequisite-constrain")]
        //public async Task<IActionResult> DeletePrerequisiteSubjectsFromPrerequisiteConstrain(
        //    [FromQuery] Guid prerequisiteConstrainId,
        //    [FromBody] List<Guid> prerequisiteSubjectIds)
        //{
        //    var response = await _prerequisiteSubjectService.DeletePrerequisiteSubjectsFromPrerequisiteConstrain(prerequisiteConstrainId, prerequisiteSubjectIds);
        //    if (!response.IsSucess)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-all-prerequisite-subjects-from-prerequisite-constrain")]
        public async Task<IActionResult> DeleteAllPrerequisiteSubjectsFromPrerequisiteConstrain([FromQuery] Guid prerequisiteConstrainId)
        {
            var response = await _prerequisiteSubjectService.DeleteAllPrerequisiteSubjectsFromPrerequisiteConstrain(prerequisiteConstrainId);
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
