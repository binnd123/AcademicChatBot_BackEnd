using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.Students;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/admins")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;

        public AdminController(IStudentService studentService, IUserService userService)
        {
            _studentService = studentService;
            _userService = userService;
        }

        // POST /api/admins
        [HttpPost]
        public async Task<IActionResult> CreateAdmin()
        {
            var result = await _userService.CreateAdminIfNotExistsAsync();
            if (result.IsSucess)
                return Ok(result);
            return BadRequest(result);
        }

        // GET /api/students
        [Authorize(Roles = "Admin")]
        [HttpGet("/api/students")]
        public async Task<IActionResult> GetStudents(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = "",
            [FromQuery] SortBy sortBy = SortBy.Default,
            [FromQuery] SortType sortType = SortType.Ascending,
            [FromQuery] bool isDelete = false
            )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;

            var response = await _studentService.GetAllStudents(pageNumber, pageSize, search, sortBy, sortType, isDelete);
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
        [HttpGet("/api/students/{studentId}")]
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
        [HttpPut("/api/students/{studentId}")]
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
