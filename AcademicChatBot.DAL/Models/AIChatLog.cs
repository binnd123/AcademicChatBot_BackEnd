using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.DAL.Models
{
    public class AIChatLog
    {
        public AIChatLog()
        {
            AIChatLogId = Guid.NewGuid();
            StartTime = DateTime.Now;
            Status = StatusChat.Actived;
            CreatedAt = DateTime.Now;
            AIChatLogName = "New Chat " + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
        public Guid AIChatLogId { get; set; }
        public string AIChatLogName { get; set; }
        public TopicChat Topic { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public StatusChat Status { get; set; }
        public DateTime LastMessageTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
