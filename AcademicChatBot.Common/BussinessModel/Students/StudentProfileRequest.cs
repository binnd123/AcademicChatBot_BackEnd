using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Common.DTOs.Students
{
    public class StudentProfileRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DOB { get; set; }
        public DateTime IntakeYear { get; set; }
        public GenderType Gender { get; set; } = GenderType.Male;
    }
}
