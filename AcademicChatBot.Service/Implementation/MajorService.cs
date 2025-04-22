using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.Major;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;

namespace AcademicChatBot.Service.Implementation
{
    public class MajorService : IMajorService
    {
        private readonly IGenericRepository<Major> _majorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MajorService(IGenericRepository<Major> majorRepository, IUnitOfWork unitOfWork)
        {
            _majorRepository = majorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateMajor(CreateMajorRequest request)
        {
            Response dto = new Response();
            try
            {
                var major = new Major
                {
                    MajorId = Guid.NewGuid(),
                    MajorCode = request.MajorCode,
                    MajorName = request.MajorName,
                    StartAt = request.StartAt,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _majorRepository.Insert(major);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = major;
                dto.Message = "Major created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating the major: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteMajor(Guid majorId)
        {
            Response dto = new Response();
            try
            {
                var major = await _majorRepository.GetById(majorId);
                if (major == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Major not found";
                    return dto;
                }
                major.IsDeleted = true;
                major.DeletedAt = DateTime.UtcNow;
                await _majorRepository.Update(major);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = major;
                dto.Message = "Major deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting the major: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllMajors(int pageNumber, int pageSize, string search)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _majorRepository.GetAllDataByExpression(
                    filter: m => m.MajorName.ToLower().Contains(search.ToLower()) && !m.IsDeleted,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: m => m.MajorName,
                    isAscending: true);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Majors retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving majors: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetMajorById(Guid majorId)
        {
            Response dto = new Response();
            try
            {
                var major = await _majorRepository.GetById(majorId);
                if (major == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Major not found";
                    return dto;
                }
                dto.Data = major;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Major retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the major: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> UpdateMajor(Guid majorId, UpdateMajorRequest request)
        {
            Response dto = new Response();
            try
            {
                var major = await _majorRepository.GetById(majorId);
                if (major == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Major not found";
                    return dto;
                }

                major.MajorCode = request.MajorCode ?? major.MajorCode;
                major.MajorName = request.MajorName ?? major.MajorName;
                major.StartAt = request.StartAt ?? major.StartAt;
                major.UpdatedAt = DateTime.UtcNow;

                await _majorRepository.Update(major);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = major;
                dto.Message = "Major updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the major: " + ex.Message;
            }
            return dto;
        }
    }
}
