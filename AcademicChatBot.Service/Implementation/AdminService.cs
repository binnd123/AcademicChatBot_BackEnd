using System;
using System.Collections.Generic;
using System.Globalization;
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
using Microsoft.Extensions.Configuration;

namespace AcademicChatBot.Service.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly IGenericRepository<Message> _messageRepository;
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IGenericRepository<Major> _majorRepository;
        private readonly IGenericRepository<Program> _programRepository;
        private readonly IGenericRepository<Combo> _comboRepository;
        private readonly IGenericRepository<ProgramingOutcome> _programingOutcomeRepository;
        private readonly IGenericRepository<ProgramingLearningOutcome> _programingLearningOutcomeRepository;
        private readonly IGenericRepository<CourseLearningOutcome> _courseLearningOutcomeRepository;
        private readonly IGenericRepository<Curriculum> _curriculumRepository;
        private readonly IGenericRepository<Assessment> _assessmentRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Material> _materialRepository;
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<Tool> _toolRepository;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AdminService(IGenericRepository<Message> messageRepository, IGenericRepository<Subject> subjectRepository, IGenericRepository<Major> majorRepository, IGenericRepository<Program> programRepository, IGenericRepository<Combo> comboRepository, IGenericRepository<ProgramingOutcome> programingOutcomeRepository, IGenericRepository<ProgramingLearningOutcome> programingLearningOutcomeRepository, IGenericRepository<CourseLearningOutcome> courseLearningOutcomeRepository, IGenericRepository<Curriculum> curriculumRepository, IGenericRepository<Assessment> assessmentRepository, IGenericRepository<User> userRepository, IGenericRepository<Material> materialRepository, IGenericRepository<Student> studentRepository, IGenericRepository<Tool> toolRepository, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _messageRepository = messageRepository;
            _subjectRepository = subjectRepository;
            _majorRepository = majorRepository;
            _programRepository = programRepository;
            _comboRepository = comboRepository;
            _programingOutcomeRepository = programingOutcomeRepository;
            _programingLearningOutcomeRepository = programingLearningOutcomeRepository;
            _courseLearningOutcomeRepository = courseLearningOutcomeRepository;
            _curriculumRepository = curriculumRepository;
            _assessmentRepository = assessmentRepository;
            _userRepository = userRepository;
            _materialRepository = materialRepository;
            _studentRepository = studentRepository;
            _toolRepository = toolRepository;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
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

        public async Task<Response> GetReportsAsync()
        {
            Response dto = new Response();
            try
            {
                var students = await _studentRepository.GetAllDataByExpression(
                    filter: null,
                    pageNumber: 1,
                    pageSize: int.MaxValue);
                var subjects = await _subjectRepository.GetAllDataByExpression(
                    filter: null,
                    pageNumber: 1,
                    pageSize: int.MaxValue);
                var majors = await _majorRepository.GetAllDataByExpression(
                    filter: null,
                    pageNumber: 1,
                    pageSize: int.MaxValue);
                var programs = await _programRepository.GetAllDataByExpression(
                    filter: null,
                    pageNumber: 1,
                    pageSize: int.MaxValue);
                var combos = await _comboRepository.GetAllDataByExpression(
                    filter: null,
                    pageNumber: 1,
                    pageSize: int.MaxValue);
                var programingOutcomes = await _programingOutcomeRepository.GetAllDataByExpression(
                    filter: null,
                    pageNumber: 1,
                    pageSize: int.MaxValue);
                var programingLearningOutcomes = await _programingLearningOutcomeRepository.GetAllDataByExpression(
                    filter: null,
                    pageNumber: 1,
                    pageSize: int.MaxValue);
                var courseLearningOutcomes = await _courseLearningOutcomeRepository.GetAllDataByExpression(
                    filter: null,
                    pageNumber: 1,
                    pageSize: int.MaxValue);
                var curriculums = await _curriculumRepository.GetAllDataByExpression(
                    filter: null,
                    pageNumber: 1,
                    pageSize: int.MaxValue);
                var assessments = await _assessmentRepository.GetAllDataByExpression(
                    filter: null,
                    pageNumber: 1,
                    pageSize: int.MaxValue);
                var materials = await _materialRepository.GetAllDataByExpression(
                    filter: null,
                    pageNumber: 1,
                    pageSize: int.MaxValue);
                var messages = await _messageRepository.GetAllDataByExpression(
                    filter: null,
                    pageNumber: 1,
                    pageSize: int.MaxValue);
                var tools = await _toolRepository.GetAllDataByExpression(
                    filter: null,
                    pageNumber: 1,
                    pageSize: int.MaxValue);
                dto.Data = new AdminReportResponse()
                {
                    TotalAssessments = assessments.Items.Count(),
                    TotalCombos = combos.Items.Count(),
                    TotalCurriculums = curriculums.Items.Count(),
                    TotalCLOs = courseLearningOutcomes.Items.Count(),
                    TotalMessages = messages.Items.Count(),
                    TotalMajors = majors.Items.Count(),
                    TotalMaterials = materials.Items.Count(),
                    TotalPLOs = programingLearningOutcomes.Items.Count(),
                    TotalPOs = programingOutcomes.Items.Count(),
                    TotalPrograms = programs.Items.Count(),
                    TotalStudents = students.Items.Count(),
                    TotalSubjects = subjects.Items.Count(),
                    TotalTools = tools.Items.Count(),
                };
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Report retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving Report: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> SetUserActiveStatusAsync(Guid userId, bool isActive)
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
                userDb.IsActive = isActive;
                userDb.UpdatedAt = DateTime.Now;
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
                dto.Message = "User update successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while update the user profile";
            }
            return dto;
        }

        public async Task<Response> DeleteUser(Guid userId)
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
