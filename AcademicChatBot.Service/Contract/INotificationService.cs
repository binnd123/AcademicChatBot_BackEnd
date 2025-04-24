using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Notifications;
using AcademicChatBot.Common.BussinessModel.Subjects;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Contract
{
    public interface INotificationService
    {
        Task<Response> GetAllNotificationsOfUser(Guid userId, int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDelete);
        Task<Response> GetNotificationById(Guid notificationId);
        public Task<Response> CreateNotification(CreateNotification createRequest);
        public Task<Response> UpdateNotification(Guid notificationId, UpdateNotificationRequest request);
        public Task<Response> DeleteNotification(Guid notificationId);
        public Task<Response> UpdateNotificationByCode(string notificationCode, UpdateNotificationRequest request);
        public Task<Response> DeleteNotificationByCode(string notificationCode);
    }
}
