using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class Program
    {
        [Key]
        public Guid ProgramId { get; set; }
        public string ProgramCode { get; set; } = null!;
        public string ProgramName { get; set; } = null!;
        public DateTime StartAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
