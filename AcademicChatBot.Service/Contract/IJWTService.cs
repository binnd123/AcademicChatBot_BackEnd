using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Models;
using Microsoft.AspNetCore.Http;

namespace AcademicChatBot.Service.Contract
{
    public interface IJwtService
    {
        Guid? GetStudentIdFromToken(HttpRequest request, out string errorMessage);
        Guid? GetUserIdFromToken(HttpRequest request, out string errorMessage);
        string GetRoleFromToken(HttpRequest request, out string errorMessage);
        public string GenerateAccessToken(Guid userId, RoleName role, string email, Guid? studentId = null);
        public string GenerateRefreshToken();
    }
}
