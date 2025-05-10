//using AcademicChatBot.Common.BussinessCode;
//using AcademicChatBot.Common.BussinessModel;
//using AcademicChatBot.Common.BussinessModel.Notifications;
//using AcademicChatBot.Common.BussinessModel.Tools;
//using AcademicChatBot.Common.Enum;
//using AcademicChatBot.Service.Contract;
//using AcademicChatBot.Service.Implementation;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace AcademicChatBot.API.Controllers
//{
//    [Route("api/notifications")]
//    [ApiController]
//    public class NotificationController : ControllerBase
//    {
//        private readonly INotificationService _notificationService;
//        private readonly IJwtService _jwtService;

//        public NotificationController(INotificationService notificationService, IJwtService jwtService)
//        {
//            _notificationService = notificationService;
//            _jwtService = jwtService;
//        }

//        // GET api/notification/{notificationId}
//        [Authorize]
//        [HttpGet("{notificationId}")]
//        public async Task<IActionResult> GetById(Guid notificationId)
//        {
//            var response = await _notificationService.GetNotificationById(notificationId);
//            if (response.IsSucess == false)
//            {
//                if (response.BusinessCode == BusinessCode.EXCEPTION)
//                    return StatusCode(500, response);
//                return NotFound(response);
//            }
//            return Ok(response);
//        }

//        // GET api/notification
//        [Authorize]
//        [HttpGet]
//        public async Task<IActionResult> GetAll(
//            [FromQuery] int pageNumber = 1,
//            [FromQuery] int pageSize = 5,
//            [FromQuery] string search = "",
//            [FromQuery] SortBy sortBy = SortBy.Default,
//            [FromQuery] SortType sortType = SortType.Ascending,
//            [FromQuery] bool isDeleted = false
//        )
//        {
//            pageNumber = pageNumber < 1 ? 1 : pageNumber;
//            pageSize = pageSize < 1 ? 5 : pageSize;
//            var userId = _jwtService.GetUserIdFromToken(HttpContext.Request, out var errorMessage);
//            if (userId == null) return Unauthorized(new Response
//            {
//                IsSucess = false,
//                BusinessCode = BusinessCode.AUTH_NOT_FOUND,
//                Message = errorMessage
//            });
//            var response = await _notificationService.GetAllNotificationsOfUser((Guid)userId, pageNumber, pageSize, search, sortBy, sortType, isDeleted);
//            if (response.IsSucess == false)
//            {
//                if (response.BusinessCode == BusinessCode.EXCEPTION)
//                    return StatusCode(500, response);
//                return NotFound(response);
//            }
//            return Ok(response);
//        }

//        // POST api/notification
//        [Authorize(Roles = "Admin")]
//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] CreateNotification createRequest)
//        {
//            var response = await _notificationService.CreateNotification(createRequest);
//            if (response.IsSucess == false)
//            {
//                if (response.BusinessCode == BusinessCode.EXCEPTION)
//                    return StatusCode(500, response);
//                return NotFound(response);
//            }
//            return Ok(response);
//        }

//        // PUT api/notification/{notificationId}
//        [Authorize(Roles = "Admin")]
//        [HttpPut("{notificationId}")]
//        public async Task<IActionResult> Update(Guid notificationId, UpdateNotificationRequest request)
//        {
//            var response = await _notificationService.UpdateNotification(notificationId, request);
//            if (response.IsSucess == false)
//            {
//                if (response.BusinessCode == BusinessCode.EXCEPTION)
//                    return StatusCode(500, response);
//                return NotFound(response);
//            }
//            return Ok(response);
//        }

//        // PUT api/notification/by-code/{notificationCode}
//        [Authorize(Roles = "Admin")]
//        [HttpPut("by-code/{notificationCode}")]
//        public async Task<IActionResult> UpdateByCode(string notificationCode, UpdateNotificationRequest request)
//        {
//            var response = await _notificationService.UpdateNotificationByCode(notificationCode, request);
//            if (response.IsSucess == false)
//            {
//                if (response.BusinessCode == BusinessCode.EXCEPTION)
//                    return StatusCode(500, response);
//                return NotFound(response);
//            }
//            return Ok(response);
//        }

//        // DELETE api/notification/{notificationId}
//        [Authorize(Roles = "Admin")]
//        [HttpDelete("{notificationId}")]
//        public async Task<IActionResult> Delete(Guid notificationId)
//        {
//            var response = await _notificationService.DeleteNotification(notificationId);
//            if (response.IsSucess == false)
//            {
//                if (response.BusinessCode == BusinessCode.EXCEPTION)
//                    return StatusCode(500, response);
//                return NotFound(response);
//            }
//            return Ok(response);
//        }

//        // DELETE api/notification/by-code/{notificationCode}
//        [Authorize(Roles = "Admin")]
//        [HttpDelete("by-code/{notificationCode}")]
//        public async Task<IActionResult> DeleteByCode(string notificationCode)
//        {
//            var response = await _notificationService.DeleteNotificationByCode(notificationCode);
//            if (response.IsSucess == false)
//            {
//                if (response.BusinessCode == BusinessCode.EXCEPTION)
//                    return StatusCode(500, response);
//                return NotFound(response);
//            }
//            return Ok(response);
//        }
//    }

//}
