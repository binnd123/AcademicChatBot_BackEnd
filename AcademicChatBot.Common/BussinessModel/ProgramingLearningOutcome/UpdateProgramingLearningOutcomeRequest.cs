using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.ProgramingLearningOutcome
{
    public class UpdateProgramingLearningOutcomeRequest
    {
        public string? ProgramingLearningOutcomeCode { get; set; }
        public string? ProgramingLearningOutcomeName { get; set; }
        public string? Description { get; set; }
        public Guid? CurriculumId { get; set; }
    }
}
