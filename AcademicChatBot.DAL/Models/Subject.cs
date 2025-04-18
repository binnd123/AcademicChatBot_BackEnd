using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class Subject
    {
        [Key]
        public Guid SubjectId { get; set; }
        public string SubjectCode { get; set; } = null!;
        public string SubjectName { get; set; } = null!;
        public int SessionNo { get; set; }
        public string DecisionNo { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public bool IsApproved { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int NoCredit { get; set; } = 3;
        public DateTime ApprovedDate { get; set; }
        public string SyllabusName { get; set; } = null!;
        public string DegreeLevel { get; set; } = "Bachelor";
        public string TimeAllocation { get; set; } = "Study hour (150h) = 45h contact hours + 105h self-study";
        public string Description { get; set; } = null!;
        public string StudentTasks { get; set; } = null!;
        public double ScoringScale { get; set; } = 10;
        public double MinAvgMarkToPass { get; set; } = 5;
        public string Note { get; set; } = string.Empty;
        public Guid? CurriculumId { get; set; }
        [ForeignKey(nameof(CurriculumId))]
        public Curriculum? Curriculum { get; set; }
    }
}
