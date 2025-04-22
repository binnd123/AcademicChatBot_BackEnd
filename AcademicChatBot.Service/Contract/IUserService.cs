using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Accounts;
using Microsoft.AspNetCore.Http;

namespace AcademicChatBot.Service.Contract
{
    public interface IUserService
    {
        public Task<Response> SignUp(AccountSignUpRequest signUpRequest);
        public Task<Response> Login(AccountLoginRequest loginRequest);
        public Task<Response> HandleRefreshToken(string refreshToken);
        public Task<Response> GoogleLogin(AccountLoginGoogleRequest request);
        public Task<Response> CreateAdminIfNotExistsAsync();
        public Task<Response> GetUserProfile(HttpRequest request);
        public Task<Response> UpdateUserProfile(Guid? userId, UpdateAccountRequest request);
        public Task<Response> ChangePassword(Guid? userId, ChangePasswordRequest request);
        public Task<Response> DeleteUser(Guid? userId);
    }
}
