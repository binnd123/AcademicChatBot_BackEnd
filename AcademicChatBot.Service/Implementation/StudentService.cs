using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Accounts;
using AcademicChatBot.Common.BussinessModel.Students;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.BussinessModel.Students;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace AcademicChatBot.Service.Implementation
{
    public class StudentService : IStudentService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<Major> _majorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IGenericRepository<User> userRepository, IGenericRepository<Student> studentRepository, IGenericRepository<Major> majorRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _studentRepository = studentRepository;
            _majorRepository = majorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> GetAllStudents(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDeleted)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _studentRepository.GetAllDataByExpression(
                    filter: s => (s.StudentCode.ToLower().Contains(search.ToLower()) || s.FullName.ToLower().Contains(search.ToLower()))
                    && s.IsDeleted == isDeleted,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: s => sortBy == SortBy.Default ? null : sortBy == SortBy.Name ? s.FullName : s.StudentCode,
                    isAscending: sortType == SortType.Ascending,
                    includes: s => s.User);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Students retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving students: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetStudentProfile(Guid? studentId)
        {
            Response dto = new Response();
            try
            {
                if (studentId == null)
                {
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.IsSucess = false;
                    dto.Message = "Student not found";
                    return dto;
                }
                var studentDb = await _studentRepository.GetFirstByExpression(
                    filter: s => s.StudentId == studentId,
                    includeProperties: s => s.Major);
                if (studentDb == null)
                {
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.IsSucess = false;
                    dto.Message = "Student not found";
                    return dto;
                }
                var userDb = await _userRepository.GetById(studentDb.UserId);
                if (userDb == null)
                {
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.IsSucess = false;
                    dto.Message = "User not found";
                    return dto;
                }
                userDb.PasswordHash = null; // Remove password hash for security reasons
                userDb.ExpiredRefreshToken = null; // Remove expired refresh token for security reasons
                userDb.RefreshToken = null; // Remove refresh token for security reasons
                var studentResponse = new StudentProfileResponse
                {
                    StudentId = studentDb.StudentId,
                    FullName = studentDb.FullName,
                    Address = studentDb.Address,
                    CreatedAt = studentDb.CreatedAt,
                    UpdatedAt = studentDb.UpdatedAt,
                    DeletedAt = studentDb.DeletedAt,
                    DOB = studentDb.DOB,
                    Gender = studentDb.Gender,
                    PhoneNumber = studentDb.PhoneNumber,
                    IntakeYear = studentDb.IntakeYear,
                    IsDeleted = studentDb.IsDeleted,
                    StudentCode = studentDb.StudentCode,
                    Major = studentDb.Major,
                    User = userDb,
                };
                dto.Data = studentResponse;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Student profile retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the student profile";
            }
            return dto;
        }

        public async Task<Response> UpdateStudentProfile(Guid? studentId, StudentProfileRequest request)
        {
            Response dto = new Response();
            try
            {
                if (studentId == null)
                {
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.IsSucess = false;
                    dto.Message = "Student not found";
                    return dto;
                }
                var studentDb = await _studentRepository.GetFirstByExpression(
                    filter: s => s.StudentId == studentId,
                    includeProperties: s => s.Major);
                if (studentDb == null)
                {
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.IsSucess = false;
                    dto.Message = "Student not found";
                    return dto;
                }
                if (!string.IsNullOrEmpty(request.Address)) studentDb.Address = request.Address;
                if (!string.IsNullOrEmpty(request.FullName)) studentDb.FullName = request.FullName;
                if (!string.IsNullOrEmpty(request.PhoneNumber)) studentDb.PhoneNumber = request.PhoneNumber;
                if (!string.IsNullOrEmpty(request.MajorId.ToString()))
                {
                    var majorDb = await _majorRepository.GetById(request.MajorId);
                    if (majorDb == null)
                    {
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.IsSucess = false;
                        dto.Message = "Major not found";
                        return dto;
                    }
                    studentDb.MajorId = majorDb.MajorId;
                }
                studentDb.Gender = request.Gender;
                studentDb.DOB = request.DOB;
                studentDb.IntakeYear = request.IntakeYear;
                await _studentRepository.Update(studentDb);
                await _unitOfWork.SaveChangeAsync();
                var userDb = await _userRepository.GetById(studentDb.UserId);
                if (userDb == null)
                {
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.IsSucess = false;
                    dto.Message = "User not found";
                    return dto;
                }
                userDb.PasswordHash = null; // Remove password hash for security reasons
                userDb.ExpiredRefreshToken = null; // Remove expired refresh token for security reasons
                userDb.RefreshToken = null; // Remove refresh token for security reasons
                var studentResponse = new StudentProfileResponse
                {
                    StudentId = studentDb.StudentId,
                    FullName = studentDb.FullName,
                    Address = studentDb.Address,
                    CreatedAt = studentDb.CreatedAt,
                    UpdatedAt = studentDb.UpdatedAt,
                    DeletedAt = studentDb.DeletedAt,
                    DOB = studentDb.DOB,
                    Gender = studentDb.Gender,
                    PhoneNumber = studentDb.PhoneNumber,
                    IntakeYear = studentDb.IntakeYear,
                    IsDeleted = studentDb.IsDeleted,
                    StudentCode = studentDb.StudentCode,
                    Major = studentDb.Major,
                    User = userDb,
                };
                dto.Data = studentResponse;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Message = "Student profile updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the student profile";
            }
            return dto;
        }
    }
}