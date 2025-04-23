using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.PrerequisiteConstraint;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;

namespace AcademicChatBot.Service.Implementation
{
    public class PrerequisiteConstraintService : IPrerequisiteConstraintService
    {
        private readonly IGenericRepository<PrerequisiteConstraint> _prerequisiteConstraintRepository;
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IGenericRepository<Curriculum> _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PrerequisiteConstraintService(
            IGenericRepository<PrerequisiteConstraint> prerequisiteConstraintRepository,
            IGenericRepository<Subject> subjectRepository,
            IGenericRepository<Curriculum> curriculumRepository,
            IUnitOfWork unitOfWork)
        {
            _prerequisiteConstraintRepository = prerequisiteConstraintRepository;
            _subjectRepository = subjectRepository;
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreatePrerequisiteConstraint(CreatePrerequisiteConstraintRequest request)
        {
            Response dto = new Response();
            try
            {
                if (request.SubjectId != null)
                {
                    var subject = await _subjectRepository.GetById(request.SubjectId.Value);
                    if (subject == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Subject not found";
                        return dto;
                    }
                }

                if (request.CurriculumId != null)
                {
                    var curriculum = await _curriculumRepository.GetById(request.CurriculumId.Value);
                    if (curriculum == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Curriculum not found";
                        return dto;
                    }
                }

                var prerequisite = new PrerequisiteConstraint
                {
                    PrerequisiteConstraintId = Guid.NewGuid(),
                    PrerequisiteConstraintCode = request.PrerequisiteConstraintCode,
                    SubjectId = request.SubjectId,
                    CurriculumId = request.CurriculumId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _prerequisiteConstraintRepository.Insert(prerequisite);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = prerequisite;
                dto.Message = "Prerequisite constraint created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating the prerequisite constraint: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllPrerequisiteConstraints(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDelete)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _prerequisiteConstraintRepository.GetAllDataByExpression(
                    filter: p => p.PrerequisiteConstraintCode.ToLower().Contains(search.ToLower()) && p.IsDeleted == isDelete,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: p => p.PrerequisiteConstraintCode,
                    isAscending: sortType == SortType.Ascending,
                    includes: p => new { p.Subject, p.Curriculum});

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Prerequisite constraints retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving prerequisite constraints: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetPrerequisiteConstraintById(Guid id)
        {
            Response dto = new Response();
            try
            {
                var prerequisite = await _prerequisiteConstraintRepository.GetById(id);

                if (prerequisite == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Prerequisite constraint not found";
                    return dto;
                }

                dto.Data = prerequisite;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Prerequisite constraint retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the prerequisite constraint: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> UpdatePrerequisiteConstraint(Guid id, UpdatePrerequisiteConstraintRequest request)
        {
            Response dto = new Response();
            try
            {
                if (request.SubjectId != null)
                {
                    var subject = await _subjectRepository.GetById(request.SubjectId.Value);
                    if (subject == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Subject not found";
                        return dto;
                    }
                }

                if (request.CurriculumId != null)
                {
                    var curriculum = await _curriculumRepository.GetById(request.CurriculumId.Value);
                    if (curriculum == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Curriculum not found";
                        return dto;
                    }
                }

                var prerequisite = await _prerequisiteConstraintRepository.GetById(id);
                if (prerequisite == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Prerequisite constraint not found";
                    return dto;
                }

                prerequisite.PrerequisiteConstraintCode = request.PrerequisiteConstraintCode ?? prerequisite.PrerequisiteConstraintCode;
                prerequisite.SubjectId = request.SubjectId ?? prerequisite.SubjectId;
                prerequisite.CurriculumId = request.CurriculumId ?? prerequisite.CurriculumId;
                prerequisite.UpdatedAt = DateTime.UtcNow;

                await _prerequisiteConstraintRepository.Update(prerequisite);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = prerequisite;
                dto.Message = "Prerequisite constraint updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the prerequisite constraint: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeletePrerequisiteConstraint(Guid id)
        {
            Response dto = new Response();
            try
            {
                var prerequisite = await _prerequisiteConstraintRepository.GetById(id);
                if (prerequisite == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Prerequisite constraint not found";
                    return dto;
                }

                prerequisite.IsDeleted = true;
                prerequisite.DeletedAt = DateTime.UtcNow;

                await _prerequisiteConstraintRepository.Update(prerequisite);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = prerequisite;
                dto.Message = "Prerequisite constraint deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting the prerequisite constraint: " + ex.Message;
            }
            return dto;
        }
    }
}
