using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.ProgramingLearningOutcome
{
    public class CreateProgramingLearningOutcomeRequest
    {
        public string ProgramingLearningOutcomeCode { get; set; } = null!;
        public string ProgramingLearningOutcomeName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid CurriculumId { get; set; }
    }
}
