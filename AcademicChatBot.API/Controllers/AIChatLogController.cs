using System.IdentityModel.Tokens.Jwt;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Authorize]
    [Route("api/ai-chat-log")]
    [ApiController]
    public class AIChatLogController : ControllerBase
    {
        private readonly IAIChatLogService _aiChatLogService;
        private readonly IJwtService _jwtService;

        public AIChatLogController(IAIChatLogService aiChatLogService, IJwtService jwtService)
        {
            _aiChatLogService = aiChatLogService;
            _jwtService = jwtService;
        }

        [HttpGet]
        [Route("get-chat")]
        public async Task<IActionResult> GetChatByUserIdAsync()
        {
            var userId = _jwtService.GetUserIdFromToken(HttpContext.Request, out var errorMessage);
            if (userId == null) return Unauthorized(new Response
            {
                IsSucess = false,
                BusinessCode = BusinessCode.AUTH_NOT_FOUND,
                Message = errorMessage
            });
            var chat = await _aiChatLogService.GetChatByUserIdAsync((Guid)userId);
            if(chat.IsSucess == false)
            {
                if (chat.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, chat);
                return NotFound(chat);
            } 
                
            return Ok(chat);
        }

        [HttpGet]
        [Route("detail/{chatId:Guid}")]
        public async Task<IActionResult> GetChatByIdAsync([FromRoute] Guid chatId)
        {
            var chat = await _aiChatLogService.GetChatByIdAsync(chatId);
            if (chat.IsSucess == false)
            {
                if (chat.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, chat);
                return NotFound(chat);
            }
            return Ok(chat);
        }

        [HttpGet]
        [Route("ai-response")]
        public async Task<IActionResult> GetAiResponseAsync([FromQuery] string message)
        {
            var userId = _jwtService.GetUserIdFromToken(HttpContext.Request, out var errorMessage);
            if (userId == null) return Unauthorized(new Response
            {
                IsSucess = false,
                BusinessCode = BusinessCode.AUTH_NOT_FOUND,
                Message = errorMessage
            });

            var chat = await _aiChatLogService.GenerateResponseAsync(userId, message);
            if (chat.IsSucess == false)
            {
                if (chat.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, chat);
                return NotFound(chat);
            }
            return Ok(chat);
        }
    }
}
