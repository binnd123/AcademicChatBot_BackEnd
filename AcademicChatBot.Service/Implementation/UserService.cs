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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
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
        private readonly IStudentService _studentService;
        private readonly IConfiguration _configuration;

        public UserService(IGenericRepository<User> userRepository, IGenericRepository<Student> studentRepository, IUnitOfWork unitOfWork, IJwtService jwtService, IStudentService studentService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _studentService = studentService;
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
                var expiredRefreshToken = DateTime.Now.AddDays(double.Parse(_configuration["JwtSettings:RefreshTokenExpirationDays"]));
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
                if (signUpRequest.Role == RoleName.Admin)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.SIGN_UP_FAILED;
                    dto.Message = "Can not sign up with role Admin!";
                    return dto;
                }
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
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    UpdatedAt = DateTime.Now,
                    DeletedAt = null,
                    IsDeleted = false,
                };
                if (signUpRequest.Role == RoleName.Student)
                {
                    var student = new Student()
                    {
                        StudentId = Guid.NewGuid(),
                        StudentCode = signUpRequest.Email.ToLower().Replace("@fpt.edu.vn", ""),
                        UserId = user.UserId,
                        FullName = signUpRequest.FullName,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false,
                        DeletedAt = null,
                        Gender = GenderType.Male,
                        Address = null,
                        DOB = null,
                        PhoneNumber = null,
                        IntakeYear = null,
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
                    CreatedAt = DateTime.Now
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
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
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
            var expiredRefreshToken = DateTime.Now.AddDays(7); // ví dụ 7 ngày

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
        public async Task<Response> CreateAdminIfNotExistsAsync()
        {
            var dto = new Response();
            try
            {
                var email = _configuration["AdminAccount:Email"];
                var password = _configuration["AdminAccount:Password"];

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Admin email or password is missing in config";
                    return dto;
                }

                var adminDb = await _userRepository.GetByExpression(
                    a => a.Email == email && 
                    a.Role == RoleName.Admin && 
                    a.IsDeleted == false && 
                    a.IsActive == true);
                if (adminDb != null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXISTED_USER;
                    dto.Message = "Admin account already exists";
                    return dto;
                }

                var passHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 12);
                var admin = new User
                {
                    UserId = Guid.NewGuid(),
                    Email = email,
                    PasswordHash = passHash,
                    Role = RoleName.Admin,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                await _userRepository.Insert(admin);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.SIGN_UP_SUCCESSFULLY;
                dto.Message = "Admin account created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = ex.Message;
            }

            return dto;
        }

        public async Task<Response> GetUserProfile(HttpRequest request)
        {
            Response dto = new Response();
            try
            {

                var role = _jwtService.GetRoleFromToken(request, out var errorMessageRole);
                if (role == null)
                {
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.IsSucess = false;
                    dto.Message = errorMessageRole;
                    return dto;
                }

                var userId = _jwtService.GetUserIdFromToken(request, out var errorMessageUser);
                if (userId == null)
                {
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.IsSucess = false;
                    dto.Message = errorMessageUser;
                    return dto;
                }

                var userDb = await _userRepository.GetById(userId);
                if (userDb == null || !userDb.IsActive)
                {
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.IsSucess = false;
                    dto.Message = "User not found";
                    return dto;
                }
                var userResponse = new UserAccountResponse()
                {
                    UserId = userDb.UserId,
                    Email = userDb.Email,
                    Role = userDb.Role,
                    IsActive = userDb.IsActive,
                    CreatedAt = userDb.CreatedAt,
                    UpdatedAt = userDb.UpdatedAt,
                    IsDeleted = userDb.IsDeleted,
                    DeletedAt = userDb.DeletedAt,
                };

                if (role == RoleName.Student.ToString())
                {
                    var studentId = _jwtService.GetStudentIdFromToken(request, out var errorMessageStudent);
                    if (studentId == null)
                    {
                        dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                        dto.IsSucess = false;
                        dto.Message = errorMessageStudent;
                        return dto;
                    }

                    var studentResponse = await _studentService.GetStudentProfile(studentId);
                    if (studentResponse.IsSucess == false)
                    {
                        return studentResponse;
                    }
                    dto.IsSucess = true;
                    dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                    dto.Message = "User profile retrieved successfully";
                    dto.Data = studentResponse.Data;
                    return dto;
                }

                dto.Data = userResponse;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "User profile retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the User profile";
            }
            return dto;
        }

        public async Task<Response> UpdateUserProfile(Guid? userId, UpdateAccountRequest request)
        {
            Response dto = new Response();
            try
            {
                var userDb = await _userRepository.GetById(userId);
                if (userDb == null)
                {
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.IsSucess = false;
                    dto.Message = "User not found";
                    return dto;
                }
                if (!request.Email.ToLower().EndsWith("@fpt.edu.vn", StringComparison.OrdinalIgnoreCase))
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.INVALID_EMAIL_FPTU;
                    dto.Message = "Email is not correct fortmat FPT university";
                    return dto;
                }
                var userEmail = await _userRepository.GetFirstByExpression(a => a.Email == request.Email, null);
                if (userEmail != null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXISTED_USER;
                    dto.Message = "Email is existed";
                    return dto;
                }
                if (!string.IsNullOrEmpty(request.Email)) userDb.Email = request.Email;
                userDb.IsActive = request.IsActive;
                var userResponse = new UserAccountResponse()
                {
                    UserId = userDb.UserId,
                    Email = userDb.Email,
                    Role = userDb.Role,
                    IsActive = userDb.IsActive,
                    CreatedAt = userDb.CreatedAt,
                    UpdatedAt = userDb.UpdatedAt,
                    IsDeleted = userDb.IsDeleted,
                    DeletedAt = userDb.DeletedAt,
                };
                await _userRepository.Update(userDb);
                await _unitOfWork.SaveChangeAsync();
                dto.Data = userResponse;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Message = "User updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the user profile";
            }
            return dto;
        }

        public async Task<Response> ChangePassword(Guid? userId, ChangePasswordRequest request)
        {
            Response dto = new Response();
            try
            {
                var userDb = await _userRepository.GetById(userId);
                if (userDb == null)
                {
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.IsSucess = false;
                    dto.Message = "User not found";
                    return dto;
                }
                if (!BCrypt.Net.BCrypt.EnhancedVerify(request.OldPassword, userDb.PasswordHash))
                {
                    dto.BusinessCode = BusinessCode.WRONG_PASSWORD;
                    dto.IsSucess = false;
                    dto.Message = "Wrong password";
                    return dto;
                }
                userDb.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.NewPassword, 12);

                await _userRepository.Update(userDb);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Message = "Change password successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the new password";
            }
            return dto;
        }

        public async Task<Response> DeleteUser(Guid? userId)
        {
            Response dto = new Response();
            try
            {
                var userDb = await _userRepository.GetById(userId);
                if (userDb == null)
                {
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.IsSucess = false;
                    dto.Message = "User not found";
                    return dto;
                }
                userDb.IsDeleted = true;
                userDb.DeletedAt = DateTime.Now;
                userDb.IsActive = false;
                userDb.UpdatedAt = DateTime.Now;
                if (userDb.Role == RoleName.Student)
                {
                    var studentDb = await _studentRepository.GetByExpression(a => a.UserId == userDb.UserId);
                    if (studentDb != null)
                    {
                        studentDb.IsDeleted = true;
                        studentDb.DeletedAt = DateTime.Now;
                        studentDb.UpdatedAt = DateTime.Now;
                        await _studentRepository.Update(studentDb);
                    }
                }
                var userResponse = new UserAccountResponse()
                {
                    UserId = userDb.UserId,
                    Email = userDb.Email,
                    Role = userDb.Role,
                    IsActive = userDb.IsActive,
                    CreatedAt = userDb.CreatedAt,
                    UpdatedAt = userDb.UpdatedAt,
                    IsDeleted = userDb.IsDeleted,
                    DeletedAt = userDb.DeletedAt,
                };
                await _userRepository.Update(userDb);
                await _unitOfWork.SaveChangeAsync();
                dto.Data = userResponse;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Message = "User deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while delete the user profile";
            }
            return dto;
        }
    }
}
