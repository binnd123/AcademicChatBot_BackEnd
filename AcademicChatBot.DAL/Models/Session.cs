using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class Session
    {
        [Key]
        public Guid SessionId { get; set; }
        public int SessionNo { get; set; }
        public string Topic { get; set; } = null!;
        public string LearningTeachingType { get; set; } = "Offline";
        public string ITU { get; set; } = "TU"; 
        public string StudentMaterials { get; set; } = null!;
        public string Download { get; set; } = string.Empty;
        public string StudentTasks { get; set; } = null!;
        public string URLs { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public Guid? LearningOutcomeId { get; set; }
        [ForeignKey(nameof(LearningOutcomeId))]
        public LearningOutcome? LearningOutcome { get; set; }
        public Guid? SyllabusId { get; set; }
        [ForeignKey(nameof(SyllabusId))]
        public Syllabus? Syllabus { get; set; }
    }
}
