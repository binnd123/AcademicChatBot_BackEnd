using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Common.BussinessModel.Accounts
{
    public class AccountSignUpRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }
        [Required]
        public required string FullName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        public RoleName Role { get; set; } = RoleName.Student;
    }
}
