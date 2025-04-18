using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class ProgramingLearningOutcome
    {
        [Key]
        public Guid ProgramingLearningOutcomeId { get; set; }
        public string ProgramingLearningOutcomeCode { get; set; } = null!;
        public string ProgramingLearningOutcomeName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CurriculumId { get; set; }
        [ForeignKey(nameof(CurriculumId))]
        public Curriculum? Curriculum { get; set; }
    }
}
