using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.Programs
{
    public class CreateProgramRequest
    {
        public string ProgramCode { get; set; } = null!;
        public string ProgramName { get; set; } = null!;
        public DateTime StartAt { get; set; } = DateTime.Now;
    }
}
