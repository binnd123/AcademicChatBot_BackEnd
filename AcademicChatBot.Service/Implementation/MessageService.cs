using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.AIChatLogs;
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

        public async Task<Response> SendMessage(Guid senderId, string content)
        {
            var response = new Response();
            try
            {
                var sender = await _userRepository.GetFirstByExpression(u => u.UserId.Equals(senderId));
                if (sender == null)
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    response.Message = "Sender not found";
                    return response;
                }
                var aIChatLog = await _aIChatLogRepository.GetFirstByExpression
                    (filter: x => x.UserId == senderId &&
                    x.Status == StatusChat.Actived &&
                    x.IsDeleted == false &&
                    x.EndTime == null
                    );
                if (aIChatLog == null)
                {
                    aIChatLog = new AIChatLog
                    {
                        UserId = senderId,
                        LastMessageTime = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _aIChatLogRepository.Insert(aIChatLog);
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
                    SentTime = DateTime.UtcNow,
                };
                // Lưu tin nhắn người dùng  
                await _messageRepository.Insert(messageUserRequest);

                // Gọi IChatService để generate câu trả lời từ AI  
                var botResponse = await aIChatLogService.GenerateResponseAsync(senderId, content);

                // Tạo đối tượng tin nhắn của bot với nội dung trả lời từ AI  
                var messageBotResponse = new Message
                {
                    MessageId = Guid.NewGuid(),
                    SenderId = Guid.Empty, // hoặc sử dụng một Guid đại diện cho bot nếu có  
                    MessageContent = botResponse.Data.ToString(),
                    MessageType = MessageType.Text,
                    IsBotResponse = true,
                    AIChatLogId = aIChatLog.AIChatLogId,
                    SentTime = DateTime.UtcNow,
                };

                // Lưu tin nhắn của bot  
                await _messageRepository.Insert(messageBotResponse);
                
                // Cập nhật lại thông tin cuộc trò chuyện 
                aIChatLog.LastMessageTime = DateTime.UtcNow;
                aIChatLog.UpdatedAt = DateTime.UtcNow;
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

                response.IsSucess = true;
                response.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                response.Data = new
                {
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

        public async Task<Response> GetMessageByChatIdAsync(Guid aIChatLogId, int pageNumber, int pageSize)
        {
            Response dto = new Response();
            try
            {
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
    }
}
