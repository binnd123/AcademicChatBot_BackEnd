using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Accounts;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using Azure.Core;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Server;

namespace AcademicChatBot.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public UserService(IGenericRepository<User> userRepository, IGenericRepository<Student> studentRepository, IUnitOfWork unitOfWork, IJwtService jwtService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        public async Task<Response> Login(AccountLoginRequest loginRequest)
        {
            Response dto = new Response();
            try
            {
                var userDb = await _userRepository.GetByExpression(a => a.Email == loginRequest.Email);
                if (userDb == null)
                {
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.IsSucess = false;
                    dto.Message = "Email is not correct";
                    return dto;
                }
                var isValid = BCrypt.Net.BCrypt.EnhancedVerify(loginRequest.Password, userDb.PasswordHash);
                if (!isValid)
                {
                    dto.BusinessCode = BusinessCode.WRONG_PASSWORD;
                    dto.IsSucess = false;
                    dto.Message = "Password is not correct";
                    return dto;
                }
                Guid? studentId = null;
                if (userDb.Role == RoleName.Student)
                {
                    var studentDb = await _studentRepository.GetByExpression(a => a.UserId == userDb.UserId);
                    if (studentDb == null)
                    {
                        dto.BusinessCode = BusinessCode.ACCESS_DENIED;
                        dto.IsSucess = false;
                        dto.Message = "Student is not valid";
                        return dto;
                    }
                    studentId = studentDb.StudentId;
                }

                var accesstoken = _jwtService.GenerateAccessToken(userDb.UserId, userDb.Role, userDb.Email, studentId);
                var refreshToken = _jwtService.GenerateRefreshToken();
                var expiredRefreshToken = DateTime.Now.AddMinutes(double.Parse(_configuration["JwtSettings:RefreshTokenExpirationDays"]));
                userDb.RefreshToken = refreshToken;
                userDb.ExpiredRefreshToken = expiredRefreshToken;
                await _userRepository.Update(userDb);
                await _unitOfWork.SaveChangeAsync();
                dto.Data = new
                {
                    AccessToken = accesstoken,
                    RefreshToken = refreshToken
                };
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.SIGN_IN_SUCCESSFULLY;
                dto.Message = "Login successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = ex.Message;
            }
            return dto;
        }

        public async Task<Response> HandleRefreshToken(string refreshToken)
        {
            Response dto = new Response();
            try
            {
                var userDb = await _userRepository.GetFirstByExpression(a => a.RefreshToken == refreshToken);
                if (userDb == null)
                {
                    dto.BusinessCode = BusinessCode.INVALID_REFRESHTOKEN;
                    dto.IsSucess = false;
                    dto.Message = "Refresh token is not correct";
                }
                else if (userDb.ExpiredRefreshToken > DateTime.Now)
                {
                    var accessToken = _jwtService.GenerateAccessToken(userDb.UserId, userDb.Role, userDb.Email);
                    userDb.RefreshToken = string.Empty;
                    await _userRepository.Update(userDb);
                    await _unitOfWork.SaveChangeAsync();
                    dto.Data = new
                    {
                        AccessToken = accessToken,
                        RefreshToken = userDb.RefreshToken,
                    };
                    dto.IsSucess = true;
                    dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                    dto.Message = "Refresh token successfully";
                }
                else
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXPIRED_REFRESHTOKEN;
                    dto.Message = "Refresh token is expired";
                }
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = ex.Message;
            }
            return dto;
        }

        public async Task<Response> SignUp(AccountSignUpRequest signUpRequest)
        {
            Response dto = new Response();
            try
            {
                //if (signUpRequest.Role == RoleName.Admin)
                //{
                //    dto.IsSucess = false;
                //    dto.BusinessCode = BusinessCode.SIGN_UP_FAILED;
                //    dto.Message = "Can not sign up with role Admin!";
                //    return dto;
                //}
                if (!signUpRequest.Email.ToLower().EndsWith("@fpt.edu.vn", StringComparison.OrdinalIgnoreCase))
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.INVALID_EMAIL_FPTU;
                    dto.Message = "Email is not correct fortmat FPT university";
                    return dto;
                }
                var userDb = await _userRepository.GetFirstByExpression(a => a.Email == signUpRequest.Email, null);
                if (userDb != null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXISTED_USER;
                    dto.Message = "Email is existed";
                    return dto;
                }
                string passWordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(signUpRequest.Password, 12);

                var user = new User()
                {
                    UserId = Guid.NewGuid(),
                    Email = signUpRequest.Email.ToLower(),
                    PasswordHash = passWordHash,
                    Role = signUpRequest.Role,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    UpdatedAt = DateTime.UtcNow,
                };
                if (signUpRequest.Role == RoleName.Student)
                {
                    var student = new Student()
                    {
                        StudentId = Guid.NewGuid(),
                        StudentCode = signUpRequest.Email.ToLower().Replace("@fpt.edu.vn", ""),
                        UserId = user.UserId,
                        FullName = signUpRequest.FullName,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    };
                    await _studentRepository.Insert(student);
                }

                await _userRepository.Insert(user);
                await _unitOfWork.SaveChangeAsync();
                dto.BusinessCode = BusinessCode.SIGN_UP_SUCCESSFULLY;
                dto.IsSucess = true;
                dto.Message = "Sign up successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = ex.Message;
            }
            return dto;
        }

        public async Task<Response> GoogleLogin(AccountLoginGoogleRequest request)
        {
            Response dto = new Response();
            GoogleJsonWebSignature.Payload payload;
            try
            {
                // Validate Google token
                payload = await GoogleJsonWebSignature.ValidateAsync(request.Token);
            }
            catch (InvalidJwtException)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.INVALID_GOOGLE_TOKEN;
                dto.Message = "Invalid Google token";
                return dto;
            }

            var email = payload.Email;
            // ✅ Kiểm tra domain email
            if (!email.EndsWith("@fpt.edu.vn", StringComparison.OrdinalIgnoreCase))
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.INVALID_EMAIL_DOMAIN;
                dto.Message = "To access the system, you must login with @fpt.edu.vn account.";
                return dto;
            }
            var user = await _userRepository.GetByExpression(u => u.Email == email);

            if (user == null)
            {
                // Tạo tài khoản mới nếu chưa tồn tại
                user = new User
                {
                    UserId = Guid.NewGuid(),
                    Email = email,
                    Role = RoleName.Student, // hoặc Admin tùy thiết kế
                    PasswordHash = null,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow
                };
                string fullName = payload.Name ?? "";

                if (user.Role == RoleName.Student)
                {
                    var existingStudent = await _studentRepository.GetByExpression(s => s.UserId == user.UserId);
                    if (existingStudent == null)
                    {
                        var student = new Student()
                        {
                            StudentId = Guid.NewGuid(),
                            StudentCode = email.Split('@')[0],
                            UserId = user.UserId,
                            FullName = fullName,
                            Gender = GenderType.Male,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                        };
                        await _studentRepository.Insert(student);
                    }
                }

                await _userRepository.Insert(user);
                await _unitOfWork.SaveChangeAsync();
            }
            else
            {
                // Nếu user đã tồn tại và là tài khoản truyền thống (có mật khẩu), từ chối login bằng Google
                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXISTED_USER_WITH_PASSWORD;
                    dto.Message = "This account is registered using email/password.";
                    return dto;
                }
            }

            var studentId = user.Role == RoleName.Student
                ? (await _studentRepository.GetByExpression(s => s.UserId == user.UserId))?.StudentId
                : null;

            var accessToken = _jwtService.GenerateAccessToken(user.UserId, user.Role, user.Email, studentId);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var expiredRefreshToken = DateTime.UtcNow.AddDays(7); // ví dụ 7 ngày

            user.RefreshToken = refreshToken;
            user.ExpiredRefreshToken = expiredRefreshToken;

            await _userRepository.Update(user);
            await _unitOfWork.SaveChangeAsync();

            dto.IsSucess = true;
            dto.BusinessCode = BusinessCode.SIGN_IN_SUCCESSFULLY;
            dto.Message = "Google login successfully";
            dto.Data = new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return dto;
        }
    }
}
