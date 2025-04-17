using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.Subjects;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/subject")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [Authorize]
        [HttpGet("get-all-subjects")]
        public async Task<ResponseDTO> GetAllSubjects(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = ""
            )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            return await _subjectService.GetAllSubjects(pageNumber, pageSize, search);
        }
        [Authorize]
        [HttpGet("get-subject/{id}")]
        public async Task<ResponseDTO> GetSubjectById(Guid id)
        {
            return await _subjectService.GetSubjectById(id);
        }
        [HttpPost("create-subject")]
        public async Task<ResponseDTO> CreateSubject([FromBody] CreateSubjectRequest request)
        {
            return await _subjectService.CreateSubject(request);
        }
        [HttpPut("update-subject/{id}")]
        public async Task<ResponseDTO> UpdateSubject(Guid id, UpdateSubjectRequest request)
        {
            return await _subjectService.UpdateSubject(id, request);
        }
        [HttpPut("delete-subject/{id}")]
        public async Task<ResponseDTO> DeleteSubject(Guid id)
        {
            return await _subjectService.DeleteSubject(id);
        }
    }
}
