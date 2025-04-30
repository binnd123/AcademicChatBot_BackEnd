using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.Accounts;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthenticateController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] AccountSignUpRequest request)
        {
            var response = await _userService.SignUp(request);
            if (!response.IsSucess)
            {
                return response.BusinessCode switch
                {
                    BusinessCode.EXISTED_USER => BadRequest(response),
                    BusinessCode.EXCEPTION => StatusCode(500, response),
                    _ => BadRequest(response)
                };
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AccountLoginRequest request)
        {
            var result = await _userService.Login(request);

            if (!result.IsSucess)
            {
                return result.BusinessCode switch
                {
                    BusinessCode.AUTH_NOT_FOUND or BusinessCode.WRONG_PASSWORD =>
                        Unauthorized(result),

                    BusinessCode.ACCESS_DENIED =>
                        Forbid(), // hoặc trả về custom response nếu muốn kèm message

                    BusinessCode.EXCEPTION =>
                        StatusCode(500, result),

                    _ => BadRequest(result)
                };
            }

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var response = await _userService.HandleRefreshToken(refreshToken);
            if (!response.IsSucess)
            {
                return response.BusinessCode switch
                {
                    BusinessCode.AUTH_NOT_FOUND =>
                        Unauthorized(response),
                    BusinessCode.EXCEPTION =>
                        StatusCode(500, response),
                    _ => BadRequest(response)
                };
            }
            return Ok(response);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] AccountLoginGoogleRequest request)
        {
            var result = await _userService.GoogleLogin(request);

            if (!result.IsSucess)
            {
                return result.BusinessCode switch
                {
                    BusinessCode.INVALID_GOOGLE_TOKEN =>
                        Unauthorized(result),
                    BusinessCode.EXISTED_USER_WITH_PASSWORD =>
                    StatusCode(StatusCodes.Status409Conflict, result),
                    _ => BadRequest(result)
                };
            }

            return Ok(result);
        }
    }
}
