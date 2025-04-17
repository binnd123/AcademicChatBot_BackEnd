using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.BussinessCode;
using AcademicChatBot.Common.DTOs.Students;
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
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IGenericRepository<User> userRepository, IGenericRepository<Student> studentRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _studentRepository = studentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> GetStudentProfile(Guid studentId)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var studentDb = await _studentRepository.GetById(studentId);
                if (studentDb == null)
                {
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.IsSucess = false;
                    dto.Message = "Student not found";
                    return dto;
                }
                dto.Data = studentDb;
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

        public async Task<ResponseDTO> UpdateStudentProfile(Guid studentId, StudentProfileRequest request)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var studentDb = await _studentRepository.GetById(studentId);
                if (studentDb == null)
                {
                    dto.BusinessCode = BusinessCode.AUTH_NOT_FOUND;
                    dto.IsSucess = false;
                    dto.Message = "Student not found";
                    return dto;
                }
                if (!string.IsNullOrEmpty(request.Address)) studentDb.Address = request.Address;
                if (!string.IsNullOrEmpty(request.FirstName)) studentDb.FirstName = request.FirstName;
                if (!string.IsNullOrEmpty(request.LastName)) studentDb.LastName = request.LastName;
                if (!string.IsNullOrEmpty(request.PhoneNumber)) studentDb.PhoneNumber = request.PhoneNumber;
                studentDb.Gender = request.Gender;
                studentDb.DOB = request.DOB;
                studentDb.IntakeYear = request.IntakeYear;
                await _studentRepository.Update(studentDb);
                await _unitOfWork.SaveChangeAsync();    
                dto.Data = studentDb;
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