using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class Assessment
    {
        [Key]
        public Guid AssessmentId { get; set; }
        public string Category { get; set; } = null!;
        public string Type { get; set; } = "on-going";
        public int Part { get; set; }
        public double Weight { get; set; }
        public string CompletionCriteria { get; set; } = ">0";
        public string Duration { get; set; } = null!;
        public string QuestionType { get; set; } = string.Empty;
        public string NoQuestion { get; set; } = string.Empty;
        public string KnowledgeAndSkill { get; set; } = string.Empty;
        public string GradingGuide { get; set; } = string.Empty;
        public string Note { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject? Subject { get; set; }
    }
}
