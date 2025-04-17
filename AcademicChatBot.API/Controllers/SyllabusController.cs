using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.Syllabus;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    
    [Route("api/syllabus")]
    [ApiController]
    public class SyllabusController : ControllerBase
    {
        private readonly ISyllabusService _syllabusService;

        public SyllabusController(ISyllabusService syllabusService)
        {
            _syllabusService = syllabusService;
        }

        [Authorize(Roles = "Admin,Student")]
        [HttpGet("get-syllabus/{id}")]
        public async Task<ResponseDTO> GetSyllabusById(Guid id)
        {
            return await _syllabusService.GetSyllabusById(id);
        }

        [HttpGet("get-all-syllabi")]
        public async Task<ResponseDTO> GetAllSyllabi(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = ""
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            return await _syllabusService.GetAllSyllabi(pageNumber, pageSize, search);
        }

        [HttpPost("create-syllabus")]
        public async Task<ResponseDTO> CreateSyllabus([FromBody] CreateSyllabusRequest request)
        {
            return await _syllabusService.CreateSyllabus(request);
        }

        [HttpPut("update-syllabus/{id}")]
        public async Task<ResponseDTO> UpdateSyllabus(Guid id, [FromBody] UpdateSyllabusRequest request)
        {
            return await _syllabusService.UpdateSyllabus(id, request);
        }

        [HttpPut("delete-syllabus/{id}")]
        public async Task<ResponseDTO> DeleteSyllabus(Guid id)
        {
            return await _syllabusService.DeleteSyllabus(id);
        }
    }
}
