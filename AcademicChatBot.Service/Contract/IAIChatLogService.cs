using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Contract
{
    public interface IAIChatLogService
    {
        Task<Response> GetAllAIChatLogByTopic(Guid? userId, int pageNumber, int pageSize, bool isDelete, TopicChat topicChat);
        Task<Response> GetAIChatLogById(Guid? userId, Guid aIChatLogId);
        //Task<Response> GetAIChatLogActivedByUserId(Guid? userId);
        Task<Response> UpdateAIChatLog(Guid? userId, Guid aIChatLogId, string? aIChatLogName);
        Task<Response> CreateAIChatLog(Guid? userId, TopicChat topic);
        Task<Response> GenerateResponseAsync(Guid? userId, string message, TopicChat topicChat);
        Task<Response> DeleteAIChatLog(Guid? userId, Guid aIChatLogId);
    }
}
