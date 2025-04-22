using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Common.BussinessModel.Accounts
{
    public class UpdateAccountRequest
    {
        public string Email { get; set; } 
        public bool IsActive { get; set; } 
    }
}
