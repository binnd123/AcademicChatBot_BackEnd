﻿using System;
using System.Threading.Tasks;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.BussinessCode;
using AcademicChatBot.Common.DTOs.Curriculum;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;

namespace AcademicChatBot.Service.Implementation
{
    public class CurriculumService : ICurriculumService
    {
        private readonly IGenericRepository<Curriculum> _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CurriculumService(IGenericRepository<Curriculum> curriculumRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> CreateCurriculum(CreateCurriculumRequest request)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var curriculum = new Curriculum
                {
                    CurriculumId = Guid.NewGuid(),
                    CurriculumCode = request.CurriculumCode,
                    CurriculumName = request.CurriculumName,
                    Description = request.Description,
                    DecisionNo = request.DecisionNo,
                    PreRequisite = request.PreRequisite,
                    IsActive = request.IsActive,
                    IsApproved = request.IsApproved,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    MajorId = request.MajorId
                };
                await _curriculumRepository.Insert(curriculum);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = curriculum;
                dto.Message = "Curriculum created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating the curriculum: " + ex.Message;
            }
            return dto;
        }

        public async Task<ResponseDTO> DeleteCurriculum(Guid curriculumId)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var curriculum = await _curriculumRepository.GetById(curriculumId);
                if (curriculum == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Curriculum not found";
                    return dto;
                }
                curriculum.IsDeleted = true;
                await _curriculumRepository.Update(curriculum);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = curriculum;
                dto.Message = "Curriculum deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting the curriculum: " + ex.Message;
            }
            return dto;
        }

        public async Task<ResponseDTO> GetAllCurriculum(int pageNumber, int pageSize, string search)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                dto.Data = await _curriculumRepository.GetAllDataByExpression(
                    filter: c => c.CurriculumName.ToLower().Contains(search.ToLower()) && !c.IsDeleted,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: c => c.CurriculumName,
                    isAscending: true);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Curriculum retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving curriculum: " + ex.Message;
            }
            return dto;
        }

        public async Task<ResponseDTO> GetCurriculumById(Guid curriculumId)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var curriculum = await _curriculumRepository.GetById(curriculumId);
                if (curriculum == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Curriculum not found";
                    return dto;
                }
                dto.Data = curriculum;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Curriculum retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the curriculum: " + ex.Message;
            }
            return dto;
        }

        public async Task<ResponseDTO> UpdateCurriculum(Guid curriculumId, UpdateCurriculumRequest request)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var curriculum = await _curriculumRepository.GetById(curriculumId);
                if (curriculum == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Curriculum not found";
                    return dto;
                }

                curriculum.CurriculumCode = request.CurriculumCode ?? curriculum.CurriculumCode;
                curriculum.CurriculumName = request.CurriculumName ?? curriculum.CurriculumName;
                curriculum.Description = request.Description ?? curriculum.Description;
                curriculum.DecisionNo = request.DecisionNo ?? curriculum.DecisionNo;
                curriculum.PreRequisite = request.PreRequisite ?? curriculum.PreRequisite;
                curriculum.IsActive = request.IsActive;
                curriculum.IsApproved = request.IsApproved;
                curriculum.UpdatedAt = DateTime.UtcNow;
                curriculum.MajorId = request.MajorId ?? curriculum.MajorId;

                await _curriculumRepository.Update(curriculum);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = curriculum;
                dto.Message = "Curriculum updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the curriculum: " + ex.Message;
            }
            return dto;
        }
    }
}
