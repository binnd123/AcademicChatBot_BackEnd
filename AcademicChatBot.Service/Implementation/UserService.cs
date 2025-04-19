using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.Accounts;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using Azure.Core;
using Microsoft.Extensions.Configuration;

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
                };
                if (signUpRequest.Role == RoleName.Student)
                {
                    var student = new Student()
                    {
                        StudentId = Guid.NewGuid(),
                        StudentCode = signUpRequest.Email.ToLower().Replace("@fpt.edu.vn", ""),
                        UserId = user.UserId,
                        FirstName = signUpRequest.FirstName,
                        LastName = signUpRequest.LastName,
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
    }
}
