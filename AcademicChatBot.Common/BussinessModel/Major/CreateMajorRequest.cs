using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.Major
{
    public class CreateMajorRequest
    {
        public string MajorCode { get; set; } = null!;
        public string MajorName { get; set; } = null!;
        public DateTime StartAt { get; set; }
    }
}
