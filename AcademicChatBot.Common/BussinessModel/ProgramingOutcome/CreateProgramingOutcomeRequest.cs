using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.ProgramingOutcome
{
    public class CreateProgramingOutcomeRequest
    {
        public string ProgramingOutcomeCode { get; set; } = null!;
        public string ProgramingOutcomeName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid? ProgramId { get; set; }
    }
}
