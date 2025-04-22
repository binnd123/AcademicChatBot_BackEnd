using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.ProgramingOutcome
{
    public class UpdateProgramingOutcomeRequest
    {
        public string? ProgramingOutcomeCode { get; set; }
        public string? ProgramingOutcomeName { get; set; }
        public string? Description { get; set; }
        public Guid? ProgramId { get; set; }
    }
}
