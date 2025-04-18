using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.DAL.Models
{
    public class POMappingPLO
    {
        [Key]
        public Guid POMappingPLOId { get; set; }
        public Guid? ProgramingLearningOutcomeId { get; set; }
        [ForeignKey(nameof(ProgramingLearningOutcomeId))]
        public ProgramingLearningOutcome? ProgramingLearningOutcome { get; set; }
        public Guid? ProgramingOutcomeId { get; set; }
        [ForeignKey(nameof(ProgramingOutcomeId))]
        public ProgramingOutcome? ProgramingOutcome { get; set; }
    }
}
