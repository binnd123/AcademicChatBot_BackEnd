using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class Syllabus
    {
        [Key]
        public Guid SyllabusId { get; set; }
        public string SyllabusCode { get; set; } = null!;
        public string SyllabusName { get; set; } = null!;
        public string DegreeLevel { get; set; } = "Bachelor";
        public string TimeAllocation { get; set; } = "Study hour (150h) = 45h contact hours + 105h self-study";
        public string Description { get; set; } = null!;
        public string StudentTasks { get; set; } = null!;
        public string Tools { get; set; } = null!;
        public double ScoringScale { get; set; } = 10;
        public double MinAvgMarkToPass { get; set; } = 5;
        public string DecisionNo { get; set; } = null!;
        public string Note { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsApproved { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime ApprovedDate { get; set; }
        public Guid? SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject? Subject { get; set; }
    }
}
