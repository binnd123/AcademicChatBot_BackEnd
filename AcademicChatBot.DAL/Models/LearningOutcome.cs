using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class LearningOutcome
    {
        [Key]
        public Guid LearningOutcomeId { get; set; }
        public string PLOName { get; set; } = string.Empty;
        public string PLODescription { get; set; } = string.Empty;
        public int CLOName { get; set; } // CLO - Course Learning Outcome
        public string CLODetails { get; set; } = string.Empty;
        public string LOName { get; set; } = string.Empty;
        public string LODetails { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public Guid? AssessmentId { get; set; }
        [ForeignKey(nameof(AssessmentId))]
        public Assessment? Assessment { get; set; }
        public Guid? SyllabusId { get; set; }
        [ForeignKey(nameof(SyllabusId))]
        public Syllabus? Syllabus { get; set; }
        public Guid? CurriculumId { get; set; }
        [ForeignKey(nameof(CurriculumId))]
        public Curriculum? Curriculum { get; set; }
    }
}
