using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.Notifications
{
    public class UpdateNotificationRequest
    {
        public string? NotificationName { get; set; } 
        public string? NotificationCode { get; set; } 
        public string? Message { get; set; } 
    }
}
