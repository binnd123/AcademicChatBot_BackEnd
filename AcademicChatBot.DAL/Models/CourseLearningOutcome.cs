using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class CourseLearningOutcome
    {
        [Key]
        public Guid CourseLearningOutcomeId { get; set; }
        public string CourseLearningOutcomeCode { get; set; } = null!;
        public string CourseLearningOutcomeName { get; set; } = null!;
        public string CourseLearningOutcomeDetail { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject? Subject { get; set; }
    }
}
