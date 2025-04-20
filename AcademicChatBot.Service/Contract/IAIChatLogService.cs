using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.AIChatLogs;

namespace AcademicChatBot.Service.Contract
{
    public interface IAIChatLogService
    {
        Task<Guid> AddChatAsync(Guid userId, AIChatLogRequest chatRequest);
        Task<Response> GetChatByUserIdAsync(Guid userId);
        Task<Response> GetChatByIdAsync(Guid chatId);
        Task<Response> UpdateChatAsync(Guid userId, Guid chatId, AIChatLogRequest chatRequest);
        Task<Response> GenerateResponseAsync(Guid? userId, string message);
    }
}
