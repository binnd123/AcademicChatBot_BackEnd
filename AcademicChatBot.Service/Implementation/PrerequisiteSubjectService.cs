using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.PrerequisiteSubject;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;

namespace AcademicChatBot.Service.Implementation
{
    public class PrerequisiteSubjectService : IPrerequisiteSubjectService
    {
        private readonly IGenericRepository<PrerequisiteSubject> _prerequisiteSubjectRepository;
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IGenericRepository<PrerequisiteConstraint> _prerequisiteConstraintRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PrerequisiteSubjectService(
            IGenericRepository<PrerequisiteSubject> prerequisiteSubjectRepository,
            IGenericRepository<Subject> subjectRepository,
            IGenericRepository<PrerequisiteConstraint> prerequisiteConstraintRepository,
            IUnitOfWork unitOfWork)
        {
            _prerequisiteSubjectRepository = prerequisiteSubjectRepository;
            _subjectRepository = subjectRepository;
            _prerequisiteConstraintRepository = prerequisiteConstraintRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreatePrerequisiteSubject(CreatePrerequisiteSubjectRequest request)
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

                if (request.PrerequisiteSubjectId != null)
                {
                    var prerequisiteSubjectInfo = await _subjectRepository.GetById(request.PrerequisiteSubjectId.Value);
                    if (prerequisiteSubjectInfo == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Prerequisite subject info not found";
                        return dto;
                    }
                }

                if (request.PrerequisiteConstraintId != null)
                {
                    var prerequisiteConstraint = await _prerequisiteConstraintRepository.GetById(request.PrerequisiteConstraintId.Value);
                    if (prerequisiteConstraint == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Prerequisite constraint not found";
                        return dto;
                    }
                }

                var prerequisiteSubject = new PrerequisiteSubject
                {
                    Id = Guid.NewGuid(),
                    RelationGroup = request.RelationGroup,
                    ConditionType = request.ConditionType,
                    SubjectId = request.SubjectId,
                    PrerequisiteSubjectId = request.PrerequisiteSubjectId,
                    PrerequisiteConstraintId = request.PrerequisiteConstraintId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _prerequisiteSubjectRepository.Insert(prerequisiteSubject);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = prerequisiteSubject;
                dto.Message = "Prerequisite subject created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating the prerequisite subject: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllPrerequisiteSubjects(int pageNumber, int pageSize, SortBy sortBy, SortType sortType, bool isDelete)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _prerequisiteSubjectRepository.GetAllDataByExpression(
                    filter: p => p.IsDeleted == isDelete,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: p => p.RelationGroup,
                    isAscending: sortType == SortType.Ascending,
                    includes: p => new { p.Subject, p.PrerequisiteSubjectInfo, p.PrerequisiteConstraint}
                );

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Prerequisite subjects retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving prerequisite subjects: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetPrerequisiteSubjectById(Guid id)
        {
            Response dto = new Response();
            try
            {
                var prerequisiteSubject = await _prerequisiteSubjectRepository.GetById(id);

                if (prerequisiteSubject == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Prerequisite subject not found";
                    return dto;
                }

                dto.Data = prerequisiteSubject;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Prerequisite subject retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the prerequisite subject: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> UpdatePrerequisiteSubject(Guid id, UpdatePrerequisiteSubjectRequest request)
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

                if (request.PrerequisiteSubjectId != null)
                {
                    var prerequisiteSubjectInfo = await _subjectRepository.GetById(request.PrerequisiteSubjectId.Value);
                    if (prerequisiteSubjectInfo == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Prerequisite subject info not found";
                        return dto;
                    }
                }

                if (request.PrerequisiteConstraintId != null)
                {
                    var prerequisiteConstraint = await _prerequisiteConstraintRepository.GetById(request.PrerequisiteConstraintId.Value);
                    if (prerequisiteConstraint == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Prerequisite constraint not found";
                        return dto;
                    }
                }

                var existingPrerequisiteSubject = await _prerequisiteSubjectRepository.GetById(id);
                if (existingPrerequisiteSubject == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Prerequisite subject not found";
                    return dto;
                }

                existingPrerequisiteSubject.RelationGroup = request.RelationGroup ?? existingPrerequisiteSubject.RelationGroup;
                existingPrerequisiteSubject.ConditionType = request.ConditionType ?? existingPrerequisiteSubject.ConditionType;
                existingPrerequisiteSubject.SubjectId = request.SubjectId ?? existingPrerequisiteSubject.SubjectId;
                existingPrerequisiteSubject.PrerequisiteSubjectId = request.PrerequisiteSubjectId ?? existingPrerequisiteSubject.PrerequisiteSubjectId;
                existingPrerequisiteSubject.PrerequisiteConstraintId = request.PrerequisiteConstraintId ?? existingPrerequisiteSubject.PrerequisiteConstraintId;
                existingPrerequisiteSubject.UpdatedAt = DateTime.UtcNow;

                await _prerequisiteSubjectRepository.Update(existingPrerequisiteSubject);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = existingPrerequisiteSubject;
                dto.Message = "Prerequisite subject updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the prerequisite subject: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeletePrerequisiteSubject(Guid id)
        {
            Response dto = new Response();
            try
            {
                var prerequisiteSubject = await _prerequisiteSubjectRepository.GetById(id);
                if (prerequisiteSubject == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Prerequisite subject not found";
                    return dto;
                }

                prerequisiteSubject.IsDeleted = true;
                prerequisiteSubject.DeletedAt = DateTime.UtcNow;

                await _prerequisiteSubjectRepository.Update(prerequisiteSubject);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = prerequisiteSubject;
                dto.Message = "Prerequisite subject deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting the prerequisite subject: " + ex.Message;
            }
            return dto;
        }
    }
}
