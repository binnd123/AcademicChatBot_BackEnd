using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.SubjectInCurriculum;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/subject-in-curriculums")]
    [ApiController]
    public class SubjectInCurriculumController : ControllerBase
    {
        private readonly ISubjectInCurriculumService _subjectInCurriculumService;

        public SubjectInCurriculumController(ISubjectInCurriculumService subjectInCurriculumService)
        {
            _subjectInCurriculumService = subjectInCurriculumService;
        }

        // Lấy thông tin môn học trong chương trình giảng dạy theo ID
        [HttpGet("{subjectInCurriculumId}")]
        public async Task<IActionResult> GetSubjectInCurriculumById(Guid subjectInCurriculumId)
        {
            var response = await _subjectInCurriculumService.GetSubjectInCurriculumById(subjectInCurriculumId);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Lấy tất cả môn học trong chương trình giảng dạy
        [HttpGet]
        public async Task<IActionResult> GetAllSubjectsForCurriculum(
            [FromQuery] Guid curriculumId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] int semesterNo = 0)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            if (semesterNo < 1 || semesterNo > 9) semesterNo = 0;
            var response = await _subjectInCurriculumService.GetAllSubjectsForCurriculum(curriculumId, pageNumber, pageSize, semesterNo);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Lấy tất cả chương trình giảng dạy cho môn học
        [HttpGet("curriculums-for-subject")]
        public async Task<IActionResult> GetCurriculumsForSubject(
            [FromQuery] Guid subjectId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] int semesterNo = 0)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            if (semesterNo < 1 || semesterNo > 9) semesterNo = 0;
            var response = await _subjectInCurriculumService.GetAllCurriculumsForSubject(subjectId, pageNumber, pageSize, semesterNo);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Thêm môn học vào chương trình giảng dạy (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddSubjectsToCurriculum(
            [FromQuery] Guid curriculumId,
            [FromBody] List<SubjectInCurriculumRequest> requests)
        {
            var response = await _subjectInCurriculumService.AddSubjectsToCurriculum(curriculumId, requests);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return BadRequest(response);
            }
            return Ok(response);
        }

        // Thêm chương trình giảng dạy vào môn học (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost("curriculums")]
        public async Task<IActionResult> AddCurriculumsToSubject(
            [FromQuery] Guid subjectId,
            [FromBody] List<CurriculumInSubjectRequest> requests)
        {
            var response = await _subjectInCurriculumService.AddCurriculumsToSubject(subjectId, requests);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return BadRequest(response);
            }
            return Ok(response);
        }

        // Xóa các môn học khỏi chương trình giảng dạy (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpDelete("subjects-from-curriculum")]
        public async Task<IActionResult> DeleteSubjectsFromCurriculum(
            [FromQuery] Guid curriculumId,
            [FromBody] List<Guid> subjectIds)
        {
            var response = await _subjectInCurriculumService.DeleteSubjectsFromCurriculum(curriculumId, subjectIds);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Xóa chương trình giảng dạy khỏi môn học (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpDelete("curriculums-from-subject")]
        public async Task<IActionResult> DeleteCurriculumsFromSubject(
            [FromQuery] Guid subjectId,
            [FromBody] List<Guid> curriculumIds)
        {
            var response = await _subjectInCurriculumService.DeleteCurriculumsFromSubject(subjectId, curriculumIds);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Xóa tất cả môn học khỏi chương trình giảng dạy (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpDelete("all-subjects")]
        public async Task<IActionResult> DeleteAllSubjectsFromCurriculum([FromQuery] Guid curriculumId)
        {
            var response = await _subjectInCurriculumService.DeleteAllSubjectsFromCurriculum(curriculumId);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Xóa tất cả chương trình giảng dạy khỏi môn học (Admin only)
        [Authorize(Roles = "Admin")]
        [HttpDelete("all-curriculums")]
        public async Task<IActionResult> DeleteAllCurriculumsFromSubject([FromQuery] Guid subjectId)
        {
            var response = await _subjectInCurriculumService.DeleteAllCurriculumsFromSubject(subjectId);
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
