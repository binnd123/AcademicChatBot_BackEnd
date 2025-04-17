using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.Accounts;
using AcademicChatBot.DAL.Models;

namespace AcademicChatBot.Service.Contract
{
    public interface IJWTService
    {
        public string GenerateAccessToken(Guid userId, string role, string email);
        public string GenerateRefreshToken();
    }
}
