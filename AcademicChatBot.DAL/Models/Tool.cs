using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class Tool
    {
        [Key]
        public Guid ToolId { get; set; }
        public string ToolCode { get; set; } = null!;
        public string ToolName { get; set; } = string.Empty;
        public string Description { get; set; } = null!;
        public string Author { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public string Note { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
