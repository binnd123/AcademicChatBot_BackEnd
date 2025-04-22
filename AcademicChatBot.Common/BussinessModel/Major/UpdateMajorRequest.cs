using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Common.BussinessModel.Major
{
    public class UpdateMajorRequest
    {
        public string? MajorCode { get; set; }
        public string? MajorName { get; set; }
        public DateTime? StartAt { get; set; }
    }
}
