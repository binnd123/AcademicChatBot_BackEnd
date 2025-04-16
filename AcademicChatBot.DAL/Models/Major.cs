using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class Major
    {
        [Key]
        public Guid MajorId { get; set; }
        public string MajorCode { get; set; } = null!;
        public string MajorName { get; set; } = null!;
        public DateTime StartAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
