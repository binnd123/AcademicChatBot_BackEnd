using AcademicChatBot.DAL.Models;
using System.Linq.Expressions;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;

namespace AcademicChatBot.API.Controllers
{
    [Authorize]
    [Route("api/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IJwtService _jwtService;

        public MessageController(IMessageService messageService, IJwtService jwtService)
        {
            _messageService = messageService;
            _jwtService = jwtService;
        }

        [HttpGet]
        [Route("get-message")]
        public async Task<IActionResult> GetMessageByChatId(
             [FromQuery] Guid aIChatLogId,
             [FromQuery] int pageNumber = 1,
             [FromQuery] int pageSize = 5)
        {
            // Đảm bảo pageIndex và pageSize hợp lệ
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 5;

            var response = await _messageService.GetMessageByChatIdAsync(aIChatLogId, pageNumber, pageSize);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("send-message")]
        public async Task<IActionResult> SendMessage(
            [FromQuery] string content)
        {
            var senderId = _jwtService.GetUserIdFromToken(HttpContext.Request, out var errorMessage);
            if (senderId == null || senderId == Guid.Empty) return Unauthorized(new Response
            {
                IsSucess = false,
                BusinessCode = BusinessCode.AUTH_NOT_FOUND,
                Message = errorMessage
            });
            var response = await _messageService.SendMessage((Guid)senderId, content);
            if (!response.IsSucess)
            {
                return response.BusinessCode switch
                {
                    BusinessCode.AUTH_NOT_FOUND => Unauthorized(response),
                    BusinessCode.EXCEPTION => StatusCode(500, response),
                    _ => BadRequest(response)
                };
            }
            return Ok(response);
        }
    }
}
