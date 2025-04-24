using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.Notifications
{
    public class CreateNotification
    {
        public List<Guid> UserIds { get; set; }
        public CreateNotificationRequest Request { get; set; }
    }
}
