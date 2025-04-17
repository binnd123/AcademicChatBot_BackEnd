using System;
using System.Threading.Tasks;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.BussinessCode;
using AcademicChatBot.Common.DTOs.Syllabus;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;

namespace AcademicChatBot.Service.Implementation
{
    public class SyllabusService : ISyllabusService
    {
        private readonly IGenericRepository<Syllabus> _syllabusRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SyllabusService(IGenericRepository<Syllabus> syllabusRepository, IUnitOfWork unitOfWork)
        {
            _syllabusRepository = syllabusRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> CreateSyllabus(CreateSyllabusRequest request)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var syllabus = new Syllabus
                {
                    SyllabusId = Guid.NewGuid(),
                    SyllabusCode = request.SyllabusCode,
                    SyllabusName = request.SyllabusName,
                    DegreeLevel = request.DegreeLevel,
                    TimeAllocation = request.TimeAllocation,
                    Description = request.Description,
                    StudentTasks = request.StudentTasks,
                    Tools = request.Tools,
                    ScoringScale = request.ScoringScale,
                    MinAvgMarkToPass = request.MinAvgMarkToPass,
                    DecisionNo = request.DecisionNo,
                    Note = request.Note,
                    IsActive = request.IsActive,
                    IsApproved = request.IsApproved,
                    IsDeleted = false,
                    ApprovedDate = request.ApprovedDate,
                    SubjectId = request.SubjectId
                };
                await _syllabusRepository.Insert(syllabus);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = syllabus;
                dto.Message = "Syllabus created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating the syllabus: " + ex.Message;
            }
            return dto;
        }
        public async Task<ResponseDTO> DeleteSyllabus(Guid syllabusId)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var syllabus = await _syllabusRepository.GetById(syllabusId);
                if (syllabus == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Syllabus not found";
                    return dto;
                }
                syllabus.IsDeleted = true;
                await _syllabusRepository.Update(syllabus);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = syllabus;
                dto.Message = "Syllabus deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting the syllabus: " + ex.Message;
            }
            return dto;
        }
        public async Task<ResponseDTO> GetAllSyllabi(int pageNumber, int pageSize, string search)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                dto.Data = await _syllabusRepository.GetAllDataByExpression(
                    filter: s => s.SyllabusName.ToLower().Contains(search.ToLower()) && !s.IsDeleted,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: s => s.SyllabusName,
                    isAscending: true);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Syllabi retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving syllabi: " + ex.Message;
            }
            return dto;
        }
        public async Task<ResponseDTO> GetSyllabusById(Guid syllabusId)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var syllabus = await _syllabusRepository.GetById(syllabusId);
                if (syllabus == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Syllabus not found";
                    return dto;
                }
                dto.Data = syllabus;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Syllabus retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the syllabus: " + ex.Message;
            }
            return dto;
        }
        public async Task<ResponseDTO> UpdateSyllabus(Guid syllabusId, UpdateSyllabusRequest request)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var syllabus = await _syllabusRepository.GetById(syllabusId);
                if (syllabus == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Syllabus not found";
                    return dto;
                }

                syllabus.SyllabusCode = request.SyllabusCode ?? syllabus.SyllabusCode;
                syllabus.SyllabusName = request.SyllabusName ?? syllabus.SyllabusName;
                syllabus.DegreeLevel = request.DegreeLevel ?? syllabus.DegreeLevel;
                syllabus.TimeAllocation = request.TimeAllocation ?? syllabus.TimeAllocation;
                syllabus.Description = request.Description ?? syllabus.Description;
                syllabus.StudentTasks = request.StudentTasks ?? syllabus.StudentTasks;
                syllabus.Tools = request.Tools ?? syllabus.Tools;
                syllabus.ScoringScale = request.ScoringScale ?? syllabus.ScoringScale;
                syllabus.MinAvgMarkToPass = request.MinAvgMarkToPass ?? syllabus.MinAvgMarkToPass;
                syllabus.DecisionNo = request.DecisionNo ?? syllabus.DecisionNo;
                syllabus.Note = request.Note ?? syllabus.Note;
                syllabus.IsActive = request.IsActive;
                syllabus.IsApproved = request.IsApproved;
                syllabus.ApprovedDate = request.ApprovedDate ?? syllabus.ApprovedDate;
                syllabus.SubjectId = request.SubjectId ?? syllabus.SubjectId;

                await _syllabusRepository.Update(syllabus);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = syllabus;
                dto.Message = "Syllabus updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the syllabus: " + ex.Message;
            }
            return dto;
        }
    }
}
