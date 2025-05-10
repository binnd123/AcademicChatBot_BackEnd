using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.Major;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.BussinessModel.Majors;

namespace AcademicChatBot.Service.Implementation
{
    public class MajorService : IMajorService
    {
        private readonly IGenericRepository<Major> _majorRepository;
        private readonly IGenericRepository<Curriculum> _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MajorService(IGenericRepository<Major> majorRepository, IGenericRepository<Curriculum> curriculumRepository, IUnitOfWork unitOfWork)
        {
            _majorRepository = majorRepository;
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateMajor(CreateMajorRequest request)
        {
            Response dto = new Response();
            try
            {
                var majorE = await _majorRepository.GetFirstByExpression(x => x.MajorCode == request.MajorCode);
                if (majorE != null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXCEPTION;
                    dto.Message = "Major is Existed!";
                    return dto;
                }

                var major = new Major
                {
                    MajorId = Guid.NewGuid(),
                    MajorCode = request.MajorCode,
                    MajorName = request.MajorName,
                    StartAt = request.StartAt,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
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
                major.DeletedAt = DateTime.Now;
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

        public async Task<Response> GetAllMajors(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDeleted)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _majorRepository.GetAllDataByExpression(
                    filter: m => (m.MajorName.ToLower().Contains(search.ToLower()) || m.MajorCode.ToLower().Contains(search.ToLower()))
                    && m.IsDeleted == isDeleted,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: m => sortBy == SortBy.Default ? null : sortBy == SortBy.Name ? m.MajorName : m.MajorCode,
                    isAscending: sortType == SortType.Ascending);
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
                var curriculums = await _curriculumRepository.GetAllDataByExpression(
                    filter: s => s.MajorId == majorId && !s.IsDeleted,
                    pageNumber: 1,
                    pageSize: 1000,
                    orderBy: null,
                    isAscending: true,
                    includes: null);
                dto.Data = new DetailMajorResponse
                {
                    MajorId = major.MajorId,
                    MajorCode = major.MajorCode,
                    MajorName = major.MajorName,
                    StartAt = major.StartAt,
                    CreatedAt = major.CreatedAt,
                    UpdatedAt = major.UpdatedAt,
                    DeletedAt = major.DeletedAt,
                    IsDeleted = major.IsDeleted,
                    Curriculums = curriculums.Items
                };
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


                if (!string.IsNullOrEmpty(request.MajorCode)) major.MajorCode = request.MajorCode;
                if (!string.IsNullOrEmpty(request.MajorName)) major.MajorName = request.MajorName;
                major.StartAt = request.StartAt ?? major.StartAt;
                major.UpdatedAt = DateTime.Now;

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
