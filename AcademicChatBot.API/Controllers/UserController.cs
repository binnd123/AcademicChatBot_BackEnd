using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Accounts;
using AcademicChatBot.Common.BussinessModel.Students;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UserController(IUserService userService, IJwtService jwtService)
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
        [Authorize]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var response = await _userService.GetUserProfile(HttpContext.Request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }
        [Authorize(Roles = "Student")]
        [HttpPut("myself")]
        public async Task<IActionResult> UpdateStudentProfile([FromBody] UpdateAccountRequest request)
        {
            var userId = _jwtService.GetUserIdFromToken(HttpContext.Request, out var errorMessage);
            if (userId == null) return Unauthorized(new Response
            {
                IsSucess = false,
                BusinessCode = BusinessCode.AUTH_NOT_FOUND,
                Message = errorMessage
            });
            var response = await _userService.UpdateUserProfile(userId, request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = _jwtService.GetUserIdFromToken(HttpContext.Request, out var errorMessage);
            if (userId == null) return Unauthorized(new Response
            {
                IsSucess = false,
                BusinessCode = BusinessCode.AUTH_NOT_FOUND,
                Message = errorMessage
            });
            var response = await _userService.ChangePassword(userId, request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpDelete("myself")]
        public async Task<IActionResult> DeleteUser()
        {
            var userId = _jwtService.GetUserIdFromToken(HttpContext.Request, out var errorMessage);
            if (userId == null) return Unauthorized(new Response
            {
                IsSucess = false,
                BusinessCode = BusinessCode.AUTH_NOT_FOUND,
                Message = errorMessage
            });
            var response = await _userService.DeleteUser(userId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
