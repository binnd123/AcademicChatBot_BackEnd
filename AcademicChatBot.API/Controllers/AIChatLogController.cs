using System.IdentityModel.Tokens.Jwt;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Combo;
using AcademicChatBot.Common.BussinessModel.Tools;
using AcademicChatBot.Common.Enum;
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

        [HttpPost]
        [Route("ai-prototype")]
        public async Task<IActionResult> GetAiResponseAsync([FromQuery] string message)
        {
            var userId = _jwtService.GetUserIdFromToken(HttpContext.Request, out var errorMessage);
            if (userId == null) return Unauthorized(new Response
            {
                IsSucess = false,
                BusinessCode = BusinessCode.AUTH_NOT_FOUND,
                Message = errorMessage
            });

            var chat = await _aiChatLogService.GenerateResponseAsync(userId, message, TopicChat.Default);
            if (chat.IsSucess == false)
            {
                if (chat.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, chat);
                return NotFound(chat);
            }
            return Ok(chat);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAIChatLogByTopic(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] bool isDelete = false,
            [FromQuery] TopicChat topicChat = TopicChat.Default
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
            var response = await _aiChatLogService.GetAllAIChatLogByTopic(userId, pageNumber, pageSize, isDelete, topicChat);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("{aIChatLogId}")]
        public async Task<IActionResult> GetAIChatLogById(Guid aIChatLogId)
        {
            var userId = _jwtService.GetUserIdFromToken(HttpContext.Request, out var errorMessage);
            if (userId == null) return Unauthorized(new Response
            {
                IsSucess = false,
                BusinessCode = BusinessCode.AUTH_NOT_FOUND,
                Message = errorMessage
            });
            var response = await _aiChatLogService.GetAIChatLogById(userId, aIChatLogId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        //[HttpGet("get-ai-chat-log-actived")]
        //public async Task<IActionResult> GetAIChatLogActivedByUserId()
        //{
        //    var userId = _jwtService.GetUserIdFromToken(HttpContext.Request, out var errorMessage);
        //    if (userId == null) return Unauthorized(new Response
        //    {
        //        IsSucess = false,
        //        BusinessCode = BusinessCode.AUTH_NOT_FOUND,
        //        Message = errorMessage
        //    });
        //    var response = await _aiChatLogService.GetAIChatLogActivedByUserId(userId);
        //    if (response.IsSucess == false)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        [HttpPut("{aIChatLogId}")]
        public async Task<IActionResult> UpdateAIChatLog(Guid aIChatLogId, string? aIChatLogName = "")
        {
            var userId = _jwtService.GetUserIdFromToken(HttpContext.Request, out var errorMessage);
            if (userId == null) return Unauthorized(new Response
            {
                IsSucess = false,
                BusinessCode = BusinessCode.AUTH_NOT_FOUND,
                Message = errorMessage
            });
            var response = await _aiChatLogService.UpdateAIChatLog(userId, aIChatLogId, aIChatLogName);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{aIChatLogId}")]
        public async Task<IActionResult> DeleteAIChatLog(Guid aIChatLogId)
        {
            var userId = _jwtService.GetUserIdFromToken(HttpContext.Request, out var errorMessage);
            if (userId == null) return Unauthorized(new Response
            {
                IsSucess = false,
                BusinessCode = BusinessCode.AUTH_NOT_FOUND,
                Message = errorMessage
            });
            var response = await _aiChatLogService.DeleteAIChatLog(userId, aIChatLogId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAIChatLog([FromQuery] TopicChat topic = TopicChat.Default)
        {
            var userId = _jwtService.GetUserIdFromToken(HttpContext.Request, out var errorMessage);
            if (userId == null) return Unauthorized(new Response
            {
                IsSucess = false,
                BusinessCode = BusinessCode.AUTH_NOT_FOUND,
                Message = errorMessage
            });
            var response = await _aiChatLogService.CreateAIChatLog(userId, topic);
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
