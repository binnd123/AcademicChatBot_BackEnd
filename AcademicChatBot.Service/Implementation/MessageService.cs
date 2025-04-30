using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Messages;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.HubService;

namespace AcademicChatBot.Service.Implementation
{
    public class MessageService : IMessageService
    {
        private readonly IGenericRepository<Message> _messageRepository;
        private readonly IGenericRepository<AIChatLog> _aIChatLogRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAIChatLogService aIChatLogService;
        private readonly IHubService _hubService;

        public MessageService(IGenericRepository<Message> messageRepository, IGenericRepository<AIChatLog> aIChatLogRepository, IGenericRepository<User> userRepository, IUnitOfWork unitOfWork, IAIChatLogService aIChatLogService, IHubService hubService)
        {
            _messageRepository = messageRepository;
            _aIChatLogRepository = aIChatLogRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            this.aIChatLogService = aIChatLogService;
            _hubService = hubService;
        }

        public async Task<Response> SendMessage(Guid senderId, Guid? aIChatLogId, TopicChat topic, string content)
        {
            var response = new Response();
            try
            {
                var sender = await _userRepository.GetById(senderId);
                if (sender == null)
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    response.Message = "Sender not found";
                    return response;
                }
                AIChatLog? aIChatLog = null;
                if (aIChatLogId == null)
                {
                    var generatedTitle = await aIChatLogService.GenerateTiltleAsync(content);
                    aIChatLog = new AIChatLog
                    {
                        AIChatLogId = Guid.NewGuid(),
                        UserId = senderId,
                        Topic = topic,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false,
                        Status = StatusChat.Actived,
                        StartTime = DateTime.Now,
                        EndTime = null,
                        AIChatLogName = generatedTitle != null ? generatedTitle.Trim() : "#New Chat " + DateTime.Now.ToString("MMM dd, yyyy HH:mm"),
                        DeletedAt = null,
                        LastMessageTime = DateTime.Now,
                    };
                    await _aIChatLogRepository.Insert(aIChatLog);
                    await _unitOfWork.SaveChangeAsync();
                }
                else
                {
                    aIChatLog = await _aIChatLogRepository.GetFirstByExpression
                    (filter: x => x.UserId == senderId
                    && x.Status == StatusChat.Actived
                    && x.IsDeleted == false
                    && x.EndTime == null
                    && x.AIChatLogId == aIChatLogId
                    && x.Topic == topic
                    );
                    if (aIChatLog == null)
                    {
                        response.IsSucess = false;
                        response.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        response.Message = "AI Chat Log not found";
                        return response;
                    }
                    //if (aIChatLog.AIChatLogName != null && aIChatLog.AIChatLogName.Contains("#New Chat "))
                    //{
                    //    var generatedTitle = await aIChatLogService.GenerateTiltleAsync(content);
                    //    if (generatedTitle != null)
                    //    {
                    //        aIChatLog.AIChatLogName = generatedTitle.Trim();
                    //    }
                    //}
                }

                // Tạo đối tượng tin nhắn của người dùng  
                var messageUserRequest = new Message
                {
                    MessageId = Guid.NewGuid(),
                    MessageContent = content,
                    SenderId = senderId,
                    MessageType = MessageType.Text,
                    IsBotResponse = false,
                    AIChatLogId = aIChatLog.AIChatLogId,
                    SentTime = DateTime.Now,
                };
                // Lưu tin nhắn người dùng  
                await _messageRepository.Insert(messageUserRequest);

                // Gọi IChatService để generate câu trả lời từ AI  
                var botResponse = await aIChatLogService.GenerateResponseAsync(senderId, content, aIChatLog.Topic);

                // Tạo đối tượng tin nhắn của bot với nội dung trả lời từ AI  
                var messageBotResponse = new Message
                {
                    MessageId = Guid.NewGuid(),
                    SenderId = Guid.Empty, // hoặc sử dụng một Guid đại diện cho bot nếu có  
                    MessageContent = botResponse.Data.ToString(),
                    MessageType = MessageType.Text,
                    IsBotResponse = true,
                    AIChatLogId = aIChatLog.AIChatLogId,
                    SentTime = DateTime.Now,
                };

                // Lưu tin nhắn của bot  
                await _messageRepository.Insert(messageBotResponse);
                
                // Cập nhật lại thông tin cuộc trò chuyện 
                aIChatLog.LastMessageTime = DateTime.Now;
                aIChatLog.UpdatedAt = DateTime.Now;
                await _aIChatLogRepository.Update(aIChatLog);

                // Gửi câu trả lời của bot về cho client  
                await _hubService.SendMessageToUserAsync(senderId, HubMethod.LOAD_BOT_RESPONSE_MESSAGE.ToString(), messageBotResponse);
                await _unitOfWork.SaveChangeAsync();
                var messageUserResponse = new MessageResponse
                {
                    MessageId = messageUserRequest.MessageId,
                    MessageContent = messageUserRequest.MessageContent,
                    SenderId = messageUserRequest.SenderId,
                    MessageType = messageUserRequest.MessageType,
                    IsBotResponse = messageUserRequest.IsBotResponse,
                    AIChatLogId = messageUserRequest.AIChatLogId,
                    SentTime = messageUserRequest.SentTime,
                };
                var messageAIResponse = new MessageResponse
                {
                    MessageId = messageBotResponse.MessageId,
                    MessageContent = messageBotResponse.MessageContent,
                    SenderId = messageBotResponse.SenderId,
                    MessageType = messageBotResponse.MessageType,
                    IsBotResponse = messageBotResponse.IsBotResponse,
                    AIChatLogId = messageBotResponse.AIChatLogId,
                    SentTime = messageBotResponse.SentTime,
                };
                aIChatLog.User = null;
                response.IsSucess = true;
                response.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                response.Data = new
                {
                    AIChatLog = aIChatLog,
                    MessageUser = messageUserResponse,
                    MessageAI = messageAIResponse
                };
                response.Message = "Sent message successfully";
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.BusinessCode = BusinessCode.EXCEPTION;
                response.Message = "An error occurred while sending the message: " + ex.Message;
            }
            return response;
        }

        public async Task<Response> GetMessageByChatIdAsync(Guid userId, Guid aIChatLogId, int pageNumber, int pageSize)
        {
            Response dto = new Response();
            try
            {
                var user = await _userRepository.GetById(userId);
                if (user == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.Message = "User not found";
                    return dto;
                }

                var aIChatLog = await _aIChatLogRepository.GetFirstByExpression(
                    filter: x => x.UserId == userId
                    && x.AIChatLogId == aIChatLogId
                    );
                if (aIChatLog == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "AI Chat Log not found";
                    return dto;
                }    

                dto.Data = await _messageRepository.GetAllDataByExpression(
                filter: x => (x.AIChatLogId == aIChatLogId),
                pageNumber: pageNumber,
                pageSize: pageSize,
                orderBy: x => x.SentTime,
                isAscending: false,
                includes: null
                );
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Messages retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving Messages: " + ex.Message;
            }
            return dto;
        }

        //public async Task<Response> GetMessageActive(Guid? userId, int pageNumber, int pageSize)
        //{
        //    Response dto = new Response();
        //    try
        //    {
        //        var aIChatLogActive = await _aIChatLogRepository.GetFirstByExpression(
        //        filter: a => a.UserId == userId && a.IsDeleted == false && a.Status == StatusChat.Actived && a.EndTime == null); 
        //        if (aIChatLogActive == null)
        //        {
        //            dto.IsSucess = true;
        //            dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
        //            dto.Message = "No Message found";
        //            return dto;
        //        }

        //        dto.Data = await _messageRepository.GetAllDataByExpression(
        //        filter: x => (x.AIChatLogId == aIChatLogActive.AIChatLogId),
        //        pageNumber: pageNumber,
        //        pageSize: pageSize,
        //        orderBy: x => x.SentTime,
        //        isAscending: false,
        //        includes: null
        //        );
        //        dto.IsSucess = true;
        //        dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
        //        dto.Message = "Messages retrieved successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        dto.IsSucess = false;
        //        dto.BusinessCode = BusinessCode.EXCEPTION;
        //        dto.Message = "An error occurred while retrieving Messages: " + ex.Message;
        //    }
        //    return dto;
        //}
    }
}
