using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Notifications;
using AcademicChatBot.Common.BussinessModel.Tools;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IJwtService _jwtService;

        public NotificationController(INotificationService notificationService, IJwtService jwtService)
        {
            _notificationService = notificationService;
            _jwtService = jwtService;
        }

        [Authorize]
        [HttpGet("get-notification-by-id/{notificationId}")]
        public async Task<IActionResult> GetNotificationById(Guid notificationId)
        {
            var response = await _notificationService.GetNotificationById(notificationId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("get-all-notifications")]
        public async Task<IActionResult> GetAllNotificationsOfUser(
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
            var userId = _jwtService.GetUserIdFromToken(HttpContext.Request, out var errorMessage);
            if (userId == null) return Unauthorized(new Response
            {
                IsSucess = false,
                BusinessCode = BusinessCode.AUTH_NOT_FOUND,
                Message = errorMessage
            });
            var response = await _notificationService.GetAllNotificationsOfUser((Guid)userId, pageNumber, pageSize, search, sortBy, sortType, isDelete);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-notifications")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotification createRequest)
        {
            var response = await _notificationService.CreateNotification(createRequest);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-notification/{notificationId}")]
        public async Task<IActionResult> UpdateNotification(Guid notificationId, UpdateNotificationRequest request)
        {
            var response = await _notificationService.UpdateNotification(notificationId, request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-notification-by-code/{notificationCode}")]
        public async Task<IActionResult> UpdateNotificationByCode(string notificationCode, UpdateNotificationRequest request)
        {
            var response = await _notificationService.UpdateNotificationByCode(notificationCode, request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-notification/{notificationId}")]
        public async Task<IActionResult> DeleteNotification(Guid notificationId)
        {
            var response = await _notificationService.DeleteNotification(notificationId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-notifications-by-code/{notificationCode}")]
        public async Task<IActionResult> DeleteNotificationByCode(string notificationCode)
        {
            var response = await _notificationService.DeleteNotificationByCode(notificationCode);
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
