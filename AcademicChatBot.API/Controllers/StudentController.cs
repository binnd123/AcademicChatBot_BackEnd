using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.Students;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Authorize(Roles = "Student")]
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpGet("get-student-profile/{studentId}")]
        public async Task<ResponseDTO> GetStudentProfile(Guid studentId)
        {
            return await _studentService.GetStudentProfile(studentId);
        }
        [HttpPut("update-student-profile/{studentId}")]
        public async Task<ResponseDTO> UpdateStudentProfile(Guid studentId, [FromBody] StudentProfileRequest request)
        {
            return await _studentService.UpdateStudentProfile(studentId, request);
        }
    }
}
