using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.Programs
{
    public class UpdateProgramRequest
    {
        public string ProgramCode { get; set; } 
        public string ProgramName { get; set; } 
        public DateTime StartAt { get; set; }
    }
}
