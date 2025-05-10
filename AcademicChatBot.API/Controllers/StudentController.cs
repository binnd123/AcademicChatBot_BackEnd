using System;
using System.IdentityModel.Tokens.Jwt;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Students;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/students")]
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
        [Authorize(Roles = "Student")]
        [HttpPut]
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

        // GET /api/students
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetStudents(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = "",
            [FromQuery] SortBy sortBy = SortBy.Default,
            [FromQuery] SortType sortType = SortType.Ascending,
            [FromQuery] bool isDeleted = false
            )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;

            var response = await _studentService.GetAllStudents(pageNumber, pageSize, search, sortBy, sortType, isDeleted);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // GET /api/students/{studentId}
        [Authorize(Roles = "Admin")]
        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudentById(Guid studentId)
        {
            var response = await _studentService.GetStudentProfile(studentId);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // PUT /api/students/{studentId}
        [Authorize(Roles = "Admin")]
        [HttpPut("{studentId}")]
        public async Task<IActionResult> UpdateStudent(Guid studentId, [FromBody] StudentProfileRequest request)
        {
            var response = await _studentService.UpdateStudentProfile(studentId, request);
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
