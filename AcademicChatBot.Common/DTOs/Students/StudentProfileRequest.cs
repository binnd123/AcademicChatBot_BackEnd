using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool Gender { get; set; }
    }
}
