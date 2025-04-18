using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.Accounts;

namespace AcademicChatBot.Service.Contract
{
    public interface IUserService
    {
        public Task<Response> SignUp(AccountSignUpRequest signUpRequest);
        public Task<Response> Login(AccountLoginRequest loginRequest);
        public Task<Response> HandleRefreshToken(string refreshToken);
    }
}
