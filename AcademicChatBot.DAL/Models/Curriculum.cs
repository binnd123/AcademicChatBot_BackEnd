using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class Curriculum
    {
        [Key]
        public Guid CurriculumId { get; set; }
        public string CurriculumCode { get; set; } = null!;
        public string CurriculumName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string DecisionNo { get; set; } = null!;
        public string PreRequisite { get; set; } = "None";
        public bool IsActive { get; set; } = true;
        public bool IsApproved { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? MajorId { get; set; }
        [ForeignKey(nameof(MajorId))]
        public Major? Major { get; set; }
    }
}
