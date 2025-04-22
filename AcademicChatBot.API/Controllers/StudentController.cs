using System;
using System.IdentityModel.Tokens.Jwt;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Students;
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

        //[HttpGet("current-student")]
        //public async Task<IActionResult> GetCurrentStudent()
        //{
        //    var studentId = _jwtService.GetStudentIdFromToken(HttpContext.Request, out var errorMessage);
        //    if (studentId == null) return Unauthorized(new Response
        //    {
        //        IsSucess = false,
        //        BusinessCode = BusinessCode.AUTH_NOT_FOUND,
        //        Message = errorMessage
        //    });
        //    var response = await _studentService.GetStudentProfile(studentId);
        //    if (response.IsSucess == false)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}
        [HttpPut("student-profile")]
        public async Task<IActionResult> UpdateStudentProfile([FromBody] StudentProfileRequest request)
        {
            var studentId = _jwtService.GetStudentIdFromToken(HttpContext.Request, out var errorMessage);
            if (studentId == null) return Unauthorized(new Response
            {
                IsSucess = false,
                BusinessCode = BusinessCode.AUTH_NOT_FOUND,
                Message = errorMessage
            });
            var response = await _studentService.UpdateStudentProfile(studentId, request);
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
