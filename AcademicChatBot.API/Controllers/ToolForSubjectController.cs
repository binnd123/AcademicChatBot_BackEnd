using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.Tools;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/tools-for-subject")]
    [ApiController]
    public class ToolForSubjectController : ControllerBase
    {
        private readonly IToolForSubjectService _toolForSubjectService;

        public ToolForSubjectController(IToolForSubjectService toolForSubjectService)
        {
            _toolForSubjectService = toolForSubjectService;
        }

        // Lấy công cụ cho môn học theo ID
        //[HttpGet("{toolForSubjectId}")]
        //public async Task<IActionResult> GetToolForSubjectById(Guid toolForSubjectId)
        //{
        //    var response = await _toolForSubjectService.GetToolForSubjectById(toolForSubjectId);
        //    if (response.IsSucess == false)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        // Lấy tất cả công cụ cho môn học
        [HttpGet]
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

        //// Lấy tất cả môn học cho công cụ
        //[HttpGet("subjects")]
        //public async Task<IActionResult> GetAllSubjectsForTool(
        //    [FromQuery] Guid toolId,
        //    [FromQuery] int pageNumber = 1,
        //    [FromQuery] int pageSize = 5
        //)
        //{
        //    pageNumber = pageNumber < 1 ? 1 : pageNumber;
        //    pageSize = pageSize < 1 ? 5 : pageSize;
        //    var response = await _toolForSubjectService.GetAllSubjectsForTool(toolId, pageNumber, pageSize);
        //    if (response.IsSucess == false)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        // Thêm môn học vào công cụ (Admin only)
        //[Authorize(Roles = "Admin")]
        //[HttpPost("subjects")]
        //public async Task<IActionResult> AddSubjectsToTool(
        //    [FromQuery] Guid toolId,
        //    [FromBody] List<Guid> subjectIds)
        //{
        //    var response = await _toolForSubjectService.AddSubjectsToTool(toolId, subjectIds);
        //    if (response.IsSucess == false)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        // Thêm công cụ vào môn học (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost("tools")]
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

        // Xóa công cụ khỏi môn học (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpDelete("tools")]
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

        // Xóa môn học khỏi công cụ (Admin only)
        //[Authorize(Roles = "Admin")]
        //[HttpDelete("subjects")]
        //public async Task<IActionResult> DeleteSubjectsFromTool(
        //    [FromQuery] Guid toolId,
        //    [FromBody] List<Guid> subjectIds)
        //{
        //    var response = await _toolForSubjectService.DeleteSubjectsFromTool(toolId, subjectIds);
        //    if (response.IsSucess == false)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        // Xóa tất cả công cụ khỏi môn học (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpDelete("tools/all")]
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

        //// Xóa tất cả môn học khỏi công cụ (Admin only)
        //[Authorize(Roles = "Admin")]
        //[HttpDelete("subjects/all")]
        //public async Task<IActionResult> DeleteAllSubjectsFromTool(
        //    [FromQuery] Guid toolId)
        //{
        //    var response = await _toolForSubjectService.DeleteAllSubjectsFromTool(toolId);
        //    if (response.IsSucess == false)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}
    }

}