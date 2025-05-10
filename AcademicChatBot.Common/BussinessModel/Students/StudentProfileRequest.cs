using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Range(2000, 2100, ErrorMessage = "IntakeYear must be from 2000 to 2100")]
        public int IntakeYear { get; set; }
        public GenderType Gender { get; set; } = GenderType.Male;
        public Guid? MajorId { get; set; } = null;
    }
}
