﻿using System;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Common.BussinessModel.Curriculum;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.Enum;
using System.Globalization;
using System.Linq.Expressions;
using AcademicChatBot.DAL.BussinessModel.Curriculums;


namespace AcademicChatBot.Service.Implementation
{
    public class CurriculumService : ICurriculumService
    {
        private readonly IGenericRepository<Curriculum> _curriculumRepository;
        private readonly IGenericRepository<Major> _majorRepository;
        private readonly IGenericRepository<Program> _programRepository;
        private readonly IGenericRepository<ProgramingLearningOutcome> _programingLearningOutcomeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CurriculumService(IGenericRepository<Curriculum> curriculumRepository, IGenericRepository<Major> majorRepository, IGenericRepository<Program> programRepository, IGenericRepository<ProgramingLearningOutcome> programingLearningOutcomeRepository, IUnitOfWork unitOfWork)
        {
            _curriculumRepository = curriculumRepository;
            _majorRepository = majorRepository;
            _programRepository = programRepository;
            _programingLearningOutcomeRepository = programingLearningOutcomeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateCurriculum(CreateCurriculumRequest request)
        {
            Response dto = new Response();
            try
            {
                var curriculumE = await _curriculumRepository.GetFirstByExpression(x => x.CurriculumCode == request.CurriculumCode);
                if (curriculumE != null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXCEPTION;
                    dto.Message = "Curriculum is Existed!";
                    return dto;
                }

                var major = await _majorRepository.GetById(request.MajorId);
                if (major == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Major not found";
                    return dto;
                }

                var program = await _programRepository.GetById(request.ProgramId);
                if (program == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Program not found";
                    return dto;
                }

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
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    MajorId = request.MajorId,
                    ProgramId = request.ProgramId
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

        public async Task<Response> DeleteCurriculum(Guid curriculumId)
        {
            Response dto = new Response();
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

        public async Task<Response> GetAllCurriculums(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDeleted)
        {
            Response dto = new Response();
            try
            {
                var includesList = new Expression<Func<Curriculum, object>>[]
                {
                    c => c.Major,
                    c => c.Program
                };

                dto.Data = await _curriculumRepository.GetAllDataByExpression(
                    filter: c => (c.CurriculumName.ToLower().Contains(search.ToLower()) || c.CurriculumCode.ToLower().Contains(search.ToLower()))
                    && c.IsDeleted == isDeleted,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: c => sortBy == SortBy.Default ? null : sortBy == SortBy.Name ? c.CurriculumName : c.CurriculumCode,
                    isAscending: sortType == SortType.Ascending,
                    includes: includesList
                );

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Curriculums retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving curriculums: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetCurriculumById(Guid curriculumId)
        {
            Response dto = new Response();
            try
            {
                var curriculum = await _curriculumRepository.GetFirstByExpression(
                    filter: x => x.CurriculumId == curriculumId,
                    includeProperties: new Expression<Func<Curriculum, object>>[]
                {
                    c => c.Major,
                    c => c.Program
                });
                if (curriculum == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Curriculum not found";
                    return dto;
                }
                var plos = await _programingLearningOutcomeRepository.GetAllDataByExpression(
                    filter: s => s.CurriculumId == curriculumId && !s.IsDeleted,
                    pageNumber: 1,
                    pageSize: 1000,
                    orderBy: null,
                    isAscending: true,
                    includes: null);
                dto.Data = new DetailCurriculumResponse
                {
                    CurriculumId = curriculum.CurriculumId,
                    CurriculumCode = curriculum.CurriculumCode,
                    CurriculumName = curriculum.CurriculumName,
                    Description = curriculum.Description,
                    DecisionNo = curriculum.DecisionNo,
                    PreRequisite = curriculum.PreRequisite,
                    IsActive = curriculum.IsActive,
                    IsApproved = curriculum.IsApproved,
                    CreatedAt = curriculum.CreatedAt,
                    UpdatedAt = curriculum.UpdatedAt,
                    DeletedAt = curriculum.DeletedAt,
                    IsDeleted = curriculum.IsDeleted,
                    Major = curriculum.Major,
                    Program = curriculum.Program,
                    ProgramingLearningOutcomes = plos.Items
                };
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

        public async Task<Response> UpdateCurriculum(Guid curriculumId, UpdateCurriculumRequest request)
        {
            Response dto = new Response();
            try
            {
                if (request.MajorId != null)
                {
                    var major = await _majorRepository.GetById(request.MajorId.Value);
                    if (major == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Major not found";
                        return dto;
                    }
                }

                if (request.ProgramId != null)
                {
                    var program = await _programRepository.GetById(request.ProgramId.Value);
                    if (program == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Program not found";
                        return dto;
                    }
                }

                var curriculum = await _curriculumRepository.GetById(curriculumId);
                if (curriculum == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Curriculum not found";
                    return dto;
                }


                if (!string.IsNullOrEmpty(request.CurriculumCode)) curriculum.CurriculumCode = request.CurriculumCode;
                if (!string.IsNullOrEmpty(request.CurriculumName)) curriculum.CurriculumName = request.CurriculumName;
                if (!string.IsNullOrEmpty(request.Description)) curriculum.Description = request.Description;
                if (!string.IsNullOrEmpty(request.DecisionNo)) curriculum.DecisionNo = request.DecisionNo;
                if (!string.IsNullOrEmpty(request.PreRequisite)) curriculum.PreRequisite = request.PreRequisite;
                curriculum.IsActive = request.IsActive;
                curriculum.IsApproved = request.IsApproved;
                curriculum.UpdatedAt = DateTime.Now;
                curriculum.MajorId = request.MajorId ?? curriculum.MajorId;
                curriculum.ProgramId = request.ProgramId ?? curriculum.ProgramId;

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
        public async Task<Response> GetCurriculumByCode(int pageNumber, int pageSize, string curriculumCode, SortBy sortBy, SortType sortType, bool isDeleted)
        {
            Response dto = new Response();
            try
            {
                var curriculum = await _curriculumRepository.GetAllDataByExpression(
                filter: c => c.CurriculumCode.Contains(curriculumCode) && c.IsDeleted == isDeleted,
                pageNumber: pageNumber,
                pageSize: pageSize,
                orderBy: c => sortBy == SortBy.Default ? null : sortBy == SortBy.Name ? c.CurriculumName : c.CurriculumCode,
                isAscending: sortType == SortType.Ascending,
                includes: c => new { c.Major, c.Program });

                if (curriculum == null || !curriculum.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No curriculum found with the specified code";
                    return dto;
                }

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Data = curriculum.Items;
                dto.Message = "curriculum retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the curriculum: " + ex.Message;
            }

            return dto;
        }
    }
}
