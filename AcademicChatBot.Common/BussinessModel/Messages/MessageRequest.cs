using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Common.BussinessModel.Messages
{
    public class MessageRequest
    {
        public Guid SenderId { get; set; }
        public string MessageContent { get; set; } = null!;
        public MessageType MessageType { get; set; }
        public Guid? AIChatLogId { get; set; }
    }
}
