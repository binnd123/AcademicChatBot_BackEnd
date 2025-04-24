﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.Notifications
{
    public class CreateNotificationRequest
    {
        public string NotificationName { get; set; } = null!;
        public string NotificationCode { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
