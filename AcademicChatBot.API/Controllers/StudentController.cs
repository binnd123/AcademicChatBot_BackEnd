using System.IdentityModel.Tokens.Jwt;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.Students;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
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
        private readonly IStudentService _studentService;
        private readonly IJwtService _jwtService;

        public StudentController(IStudentService studentService, IJwtService jwtService)
        {
            _studentService = studentService;
            _jwtService = jwtService;
        }

        [HttpGet("get-student-profile")]
        public async Task<Response> GetStudentProfile()
        {
            var studentId = _jwtService.GetStudentIdFromToken(HttpContext.Request, out var errorMessage);
            if (studentId == null) return new Response
            {
                IsSucess = false,
                BusinessCode = BusinessCode.AUTH_NOT_FOUND,
                Message = errorMessage
            };
            return await _studentService.GetStudentProfile(studentId);
        }
        [HttpPut("update-student-profile/{studentId}")]
        public async Task<Response> UpdateStudentProfile(Guid studentId, [FromBody] StudentProfileRequest request)
        {
            return await _studentService.UpdateStudentProfile(studentId, request);
        }
    }
}
