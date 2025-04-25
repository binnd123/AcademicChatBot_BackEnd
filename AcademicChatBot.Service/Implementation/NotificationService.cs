using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Notifications;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using Azure.Core;

namespace AcademicChatBot.Service.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly IGenericRepository<Notification> _notificationRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IGenericRepository<Notification> notificationRepository, IGenericRepository<User> userRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateNotification(CreateNotification createRequest)
        {
            Response dto = new Response();
            try
            {
                var request = createRequest.Request;
                var userIds = createRequest.UserIds;
                var notificationList = new List<Notification>();
                foreach (var userId in userIds)
                {
                    var user = await _userRepository.GetById(userId);
                    if (user == null) continue;
                    var existingNotification = await _notificationRepository.GetFirstByExpression(x => x.NotificationCode == request.NotificationCode && x.UserId == userId);
                    if (existingNotification != null) continue;
                    notificationList.Add(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        UserId = userId,
                        NotificationCode = request.NotificationCode,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsRead = false,
                        IsDeleted = false,
                        Message = request.Message,
                        DeletedAt = null,
                        NotificationName = request.NotificationName,
                    });
                }
                await _notificationRepository.InsertRange(notificationList);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = notificationList;
                dto.Message = "Notification created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating Notification: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteNotification(Guid notificationId)
        {
            Response dto = new Response();
            try
            {
                var notification = await _notificationRepository.GetById(notificationId);
                if (notification == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Notification not found";
                    return dto;
                }
                notification.IsDeleted = true;
                notification.DeletedAt = DateTime.Now;
                notification.UpdatedAt = DateTime.Now;
                await _notificationRepository.Update(notification);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = notification;
                dto.Message = "Notification deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting notification: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllNotificationsOfUser(Guid userId, int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDelete)
        {
            Response dto = new Response();
            try
            {
                var notifications = await _notificationRepository.GetAllDataByExpression(
                    filter: t => (t.NotificationName.ToLower().Contains(search.ToLower()) || t.NotificationCode.ToLower().Contains(search.ToLower()))
                    && t.IsDeleted == isDelete && t.UserId == userId,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: t => sortBy == SortBy.Default ? null : sortBy == SortBy.Name ? t.NotificationName : t.NotificationCode,
                    isAscending: sortType == SortType.Ascending);
                foreach (var notification in notifications.Items)
                {
                    notification.IsRead = true;
                }
                await _notificationRepository.UpdateRange(notifications.Items);
                await _unitOfWork.SaveChangeAsync();
                dto.Data = notifications;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Notifications retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving Notifications: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetNotificationById(Guid notificationId)
        {
            Response dto = new Response();
            try
            {
                var notification = await _notificationRepository.GetById(notificationId);
                if (notification == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Notification not found";
                    return dto;
                }
                notification.IsRead = true;
                await _notificationRepository.Update(notification);
                await _unitOfWork.SaveChangeAsync();
                dto.Data = notification;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Notification retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving Notification: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> UpdateNotification(Guid notificationId, UpdateNotificationRequest request)
        {
            Response dto = new Response();
            try
            {
                var notification = await _notificationRepository.GetById(notificationId);
                if (notification == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Notification not found";
                    return dto;
                }

                if (!string.IsNullOrEmpty(request.NotificationCode)) notification.NotificationCode = request.NotificationCode;
                if (!string.IsNullOrEmpty(request.NotificationName)) notification.NotificationName = request.NotificationName;
                if (!string.IsNullOrEmpty(request.Message)) notification.Message = request.Message;
                notification.UpdatedAt = DateTime.Now;

                await _notificationRepository.Update(notification);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = notification;
                dto.Message = "Notification updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating Notification: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> UpdateNotificationByCode(string notificationCode, UpdateNotificationRequest request)
        {
            Response dto = new Response();
            try
            {
                var notifications = await _notificationRepository.GetAllDataByExpression(
                    filter: n => n.NotificationCode == notificationCode,
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);
                if (notifications.Items == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Notifications not found";
                    return dto;
                }
                foreach (var notification in notifications.Items)
                {
                    if (!string.IsNullOrEmpty(request.NotificationCode)) notification.NotificationCode = request.NotificationCode;
                    if (!string.IsNullOrEmpty(request.NotificationName)) notification.NotificationName = request.NotificationName;
                    if (!string.IsNullOrEmpty(request.Message)) notification.Message = request.Message;
                    notification.UpdatedAt = DateTime.Now;
                };
                await _notificationRepository.UpdateRange(notifications.Items);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = await _notificationRepository.GetAllDataByExpression(
                    filter: n => n.NotificationCode == request.NotificationCode,
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);
                dto.Message = "Notification updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating Notification: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteNotificationByCode(string notificationCode)
        {
            Response dto = new Response();
            try
            {
                var notifications = await _notificationRepository.GetAllDataByExpression(
                    filter: n => n.NotificationCode == notificationCode,
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);
                if (notifications.Items == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Notifications not found";
                    return dto;
                }
                foreach (var notification in notifications.Items)
                {
                    notification.IsDeleted = true;
                    notification.DeletedAt = DateTime.Now;
                    notification.UpdatedAt = DateTime.Now;
                };
                await _notificationRepository.UpdateRange(notifications.Items);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = notifications;
                dto.Message = "Notifications deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting notifications: " + ex.Message;
            }
            return dto;
        }
    }
}
