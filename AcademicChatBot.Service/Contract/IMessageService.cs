using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Messages;
using AcademicChatBot.DAL.Models;

namespace AcademicChatBot.Service.Contract
{
    public interface IMessageService
    {
        Task<Response> SendMessage(Guid senderId, string content);
        Task<Response> GetMessageByChatIdAsync(Guid aIChatLogId, int pageNumber, int pageSize);
        Task<Response> GetMessageActive(Guid? userId, int pageNumber, int pageSize);
    }
}
