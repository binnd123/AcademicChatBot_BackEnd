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
        public Task<ResponseDTO> SignUp(AccountSignUpRequest signUpRequest);
        public Task<ResponseDTO> Login(AccountLoginRequest loginRequest);
        public Task<ResponseDTO> HandleRefreshToken(string refreshToken);
    }
}
