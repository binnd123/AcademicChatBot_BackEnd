using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.Accounts;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }
        [HttpPost("sign-up")]
        public async Task<Response> SignUp(AccountSignUpRequest request)
        {
            return await _service.SignUp(request);
        }

        [HttpPost("login")]
        public async Task<Response> Login(AccountLoginRequest request)
        {
            return await _service.Login(request);
        }

        [HttpPost("refresh-token")]
        public async Task<Response> RefreshToken(string refreshToken)
        {
            return await _service.HandleRefreshToken(refreshToken);
        }
    }
}
