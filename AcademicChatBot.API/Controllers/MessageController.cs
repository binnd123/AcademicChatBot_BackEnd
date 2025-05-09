﻿using AcademicChatBot.DAL.Models;
using System.Linq.Expressions;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.API.Controllers
{
    [Authorize]
    [Route("api/messages")]
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

        //[HttpGet]
        //public async Task<IActionResult> GetMessageByChatIdAsync(
        //     [FromQuery] Guid aIChatLogId,
        //     [FromQuery] int pageNumber = 1,
        //     [FromQuery] int pageSize = 10)
        //{
        //    // Đảm bảo pageIndex và pageSize hợp lệ
        //    if (pageNumber < 1) pageNumber = 1;
        //    if (pageSize < 1) pageSize = 10;
        //    var userId = _jwtService.GetUserIdFromToken(HttpContext.Request, out var errorMessage);
        //    if (userId == null) return Unauthorized(new Response
        //    {
        //        IsSucess = false,
        //        BusinessCode = BusinessCode.AUTH_NOT_FOUND,
        //        Message = errorMessage
        //    });
        //    var response = await _messageService.GetMessageByChatIdAsync((Guid)userId, aIChatLogId, pageNumber, pageSize);
        //    if (response.IsSucess == false)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        //[HttpGet]
        //[Route("get-message-active")]
        //public async Task<IActionResult> GetMessageActive(
        //     [FromQuery] int pageNumber = 1,
        //     [FromQuery] int pageSize = 10)
        //{
        //    var userId = _jwtService.GetUserIdFromToken(HttpContext.Request, out var errorMessage);
        //    if (userId == null) return Unauthorized(new Response
        //    {
        //        IsSucess = false,
        //        BusinessCode = BusinessCode.AUTH_NOT_FOUND,
        //        Message = errorMessage
        //    });
        //    // Đảm bảo pageIndex và pageSize hợp lệ
        //    if (pageNumber < 1) pageNumber = 1;
        //    if (pageSize < 1) pageSize = 10;

        //    var response = await _messageService.GetMessageActive(userId, pageNumber, pageSize);
        //    if (response.IsSucess == false)
        //    {
        //        if (response.BusinessCode == BusinessCode.EXCEPTION)
        //            return StatusCode(500, response);
        //        return NotFound(response);
        //    }
        //    return Ok(response);
        //}

        [HttpPost]
        public async Task<IActionResult> SendMessage(
            [FromQuery] Guid? aIChatLogId,
            [FromQuery] string content,
            [FromQuery] TopicChat? topic)
        {
            var senderId = _jwtService.GetUserIdFromToken(HttpContext.Request, out var errorMessage);
            if (senderId == null || senderId == Guid.Empty) return Unauthorized(new Response
            {
                IsSucess = false,
                BusinessCode = BusinessCode.AUTH_NOT_FOUND,
                Message = errorMessage
            });
            if (topic == null)
            {
                return BadRequest(new Response
                {
                    IsSucess = false,
                    BusinessCode = BusinessCode.EXCEPTION,
                    Message = "Phải chọn một Topic rõ ràng."
                });
            }
            var response = await _messageService.SendMessage((Guid)senderId, aIChatLogId, (TopicChat)topic, content);
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
