using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.DAL.Models
{
    public class Message
    {
        public Message()
        {
            MessageId = Guid.NewGuid();
            SentTime = DateTime.Now;
        }
        public Guid MessageId { get; set; }
        public Guid SenderId { get; set; }
        public string MessageContent { get; set; } = null!;
        public DateTime SentTime { get; set; }
        public MessageType MessageType { get; set; }
        public bool IsBotResponse { get; set; }
        public Guid? AIChatLogId { get; set; }
        [ForeignKey(nameof(AIChatLogId))]
        public AIChatLog? AIChatLog { get; set; }
    }
}
