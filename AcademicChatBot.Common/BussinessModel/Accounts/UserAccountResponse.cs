using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Common.BussinessModel.Accounts
{
    public class UserAccountResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } 
        public bool IsActive { get; set; } 
        public RoleName Role { get; set; } = RoleName.Student;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
