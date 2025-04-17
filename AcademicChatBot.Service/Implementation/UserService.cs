using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.Accounts;
using AcademicChatBot.Common.DTOs.BussinessCode;
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
        private readonly IJWTService _jWTService;
        private readonly IConfiguration _configuration;

        public UserService(IGenericRepository<User> userRepository, IGenericRepository<Student> studentRepository, IUnitOfWork unitOfWork, IJWTService jWTService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
            _jWTService = jWTService;
            _configuration = configuration;
        }

        public async Task<ResponseDTO> Login(AccountLoginRequest loginRequest)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var userDb = await _userRepository.GetByExpression(a => a.Email == loginRequest.Email);
                if (userDb == null)
                {
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.IsSucess = false;
                    return dto;
                }
                var isValid = BCrypt.Net.BCrypt.EnhancedVerify(loginRequest.Password, userDb.PasswordHash);
                if (!isValid)
                {
                    dto.BusinessCode = BusinessCode.WRONG_PASSWORD;
                    dto.IsSucess = false;
                    return dto;
                }

                var accesstoken = _jWTService.GenerateAccessToken(userDb.UserId, userDb.Role, userDb.Email);
                var refreshToken = _jWTService.GenerateRefreshToken();
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
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        public async Task<ResponseDTO> HandleRefreshToken(string refreshToken)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var userDb = await _userRepository.GetFirstByExpression(a => a.RefreshToken == refreshToken);
                if (userDb == null)
                {
                    dto.BusinessCode = BusinessCode.INVALID_REFRESHTOKEN;
                    dto.IsSucess = false;
                }
                else if (userDb.ExpiredRefreshToken > DateTime.Now)
                {
                    var accessToken = _jWTService.GenerateAccessToken(userDb.UserId, userDb.Role, userDb.Email);
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
                }
                else
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXPIRED_REFRESHTOKEN;
                }
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }

        public async Task<ResponseDTO> SignUp(AccountSignUpRequest signUpRequest)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                if (!signUpRequest.Email.ToLower().EndsWith("@fpt.edu.vn", StringComparison.OrdinalIgnoreCase))
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.INVALID_EMAIL_FPTU;
                    return dto;
                }
                var userDb = await _userRepository.GetFirstByExpression(a => a.Email == signUpRequest.Email, null);
                if (userDb != null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXISTED_USER;
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
                if (signUpRequest.Role.ToLower() == "student")
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
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }
    }
}
