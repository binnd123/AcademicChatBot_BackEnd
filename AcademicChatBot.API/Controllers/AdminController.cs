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
    [Route("api/admin")]
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

        [HttpPost("init-admin")]
        public async Task<IActionResult> InitAdmin()
        {
            var result = await _userService.CreateAdminIfNotExistsAsync();
            if (result.IsSucess) return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-students")]
        public async Task<IActionResult> GetAllSubjects(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = "",
            [FromQuery] SortBy sortBy = SortBy.Default,
            [FromQuery] SortType sortType = SortType.Ascending
            )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            var response = await _studentService.GetAllStudents(pageNumber, pageSize, search, sortBy, sortType);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-student-by-id/{studentId}")]
        public async Task<IActionResult> GetSubjectById(Guid studentId)
        {
            var response = await _studentService.GetStudentProfile(studentId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-student-profile/{studentId}")]
        public async Task<IActionResult> UpdateStudentProfile(Guid studentId, [FromBody] StudentProfileRequest request)
        {
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
