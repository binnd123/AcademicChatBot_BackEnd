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
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // POST /api/admins
        [HttpPost("init")]
        public async Task<IActionResult> CreateAdmin()
        {
            var result = await _adminService.CreateAdminIfNotExistsAsync();
            if (result.IsSucess)
                return Ok(result);
            return BadRequest(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("report")]
        public async Task<IActionResult> GetReports()
        {
            var response = await _adminService.GetReportsAsync();
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var response = await _adminService.DeleteUser(userId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{userId}/status")]
        public async Task<IActionResult> SetUserActiveStatus(Guid userId, [FromQuery] bool isActive = true)
        {
            var response = await _adminService.SetUserActiveStatusAsync(userId, isActive);
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
