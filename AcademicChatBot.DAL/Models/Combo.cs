using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class Combo
    {
        [Key]
        public Guid ComboId { get; set; }
        public string ComboCode { get; set; } = null!;
        public string ComboName { get; set; } = null!;
        public string Note { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsApproved { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid? ProgramId { get; set; }
        [ForeignKey(nameof(ProgramId))]
        public Program? Program { get; set; }
    }
}
