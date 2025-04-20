using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Common.BussinessModel.Students
{
    public class StudentProfileRequest
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DOB { get; set; }
        public DateTime IntakeYear { get; set; }
        public GenderType Gender { get; set; } = GenderType.Male;
    }
}
