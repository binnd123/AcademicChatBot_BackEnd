using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        //public async Task<Response> CreatePrerequisiteSubject(CreatePrerequisiteSubjectRequest request)
        //{
        //    Response dto = new Response();
        //    try
        //    {
        //        if (request.SubjectId != null)
        //        {
        //            var subject = await _subjectRepository.GetById(request.SubjectId.Value);
        //            if (subject == null)
        //            {
        //                dto.IsSucess = false;
        //                dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
        //                dto.Message = "Subject not found";
        //                return dto;
        //            }
        //        }

        //        if (request.PrerequisiteSubjectId != null)
        //        {
        //            var prerequisiteSubjectInfo = await _subjectRepository.GetById(request.PrerequisiteSubjectId.Value);
        //            if (prerequisiteSubjectInfo == null)
        //            {
        //                dto.IsSucess = false;
        //                dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
        //                dto.Message = "Prerequisite subject info not found";
        //                return dto;
        //            }
        //        }

        //        if (request.PrerequisiteConstraintId != null)
        //        {
        //            var prerequisiteConstraint = await _prerequisiteConstraintRepository.GetById(request.PrerequisiteConstraintId.Value);
        //            if (prerequisiteConstraint == null)
        //            {
        //                dto.IsSucess = false;
        //                dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
        //                dto.Message = "Prerequisite constraint not found";
        //                return dto;
        //            }
        //        }

        //        var prerequisiteSubject = new PrerequisiteSubject
        //        {
        //            Id = Guid.NewGuid(),
        //            RelationGroup = request.RelationGroup,
        //            ConditionType = request.ConditionType,
        //            SubjectId = request.SubjectId,
        //            PrerequisiteSubjectId = request.PrerequisiteSubjectId,
        //            PrerequisiteConstraintId = request.PrerequisiteConstraintId,
        //            CreatedAt = DateTime.Now,
        //            UpdatedAt = DateTime.Now,
        //            IsDeleted = false
        //        };

        //        await _prerequisiteSubjectRepository.Insert(prerequisiteSubject);
        //        await _unitOfWork.SaveChangeAsync();

        //        dto.IsSucess = true;
        //        dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
        //        dto.Data = prerequisiteSubject;
        //        dto.Message = "Prerequisite subject created successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        dto.IsSucess = false;
        //        dto.BusinessCode = BusinessCode.EXCEPTION;
        //        dto.Message = "An error occurred while creating the prerequisite subject: " + ex.Message;
        //    }
        //    return dto;
        //}

        //public async Task<Response> GetAllPrerequisiteSubjects(int pageNumber, int pageSize, SortBy sortBy, SortType sortType, bool isDelete)
        //{
        //    Response dto = new Response();
        //    try
        //    {
        //        var includesList = new Expression<Func<PrerequisiteSubject, object>>[]
        //        {
        //            p => p.Subject,
        //            p => p.PrerequisiteSubjectInfo,
        //            p => p.PrerequisiteConstraint
        //        };
        //        dto.Data = await _prerequisiteSubjectRepository.GetAllDataByExpression(
        //            filter: p => p.IsDeleted == isDelete,
        //            pageNumber: pageNumber,
        //            pageSize: pageSize,
        //            orderBy: p => p.RelationGroup,
        //            isAscending: sortType == SortType.Ascending,
        //            includes: includesList
        //        );

        //        dto.IsSucess = true;
        //        dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
        //        dto.Message = "Prerequisite subjects retrieved successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        dto.IsSucess = false;
        //        dto.BusinessCode = BusinessCode.EXCEPTION;
        //        dto.Message = "An error occurred while retrieving prerequisite subjects: " + ex.Message;
        //    }
        //    return dto;
        //}

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

        //public async Task<Response> UpdatePrerequisiteSubject(Guid id, UpdatePrerequisiteSubjectRequest request)
        //{
        //    Response dto = new Response();
        //    try
        //    {
        //        if (request.SubjectId != null)
        //        {
        //            var subject = await _subjectRepository.GetById(request.SubjectId.Value);
        //            if (subject == null)
        //            {
        //                dto.IsSucess = false;
        //                dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
        //                dto.Message = "Subject not found";
        //                return dto;
        //            }
        //        }

        //        if (request.PrerequisiteSubjectId != null)
        //        {
        //            var prerequisiteSubjectInfo = await _subjectRepository.GetById(request.PrerequisiteSubjectId.Value);
        //            if (prerequisiteSubjectInfo == null)
        //            {
        //                dto.IsSucess = false;
        //                dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
        //                dto.Message = "Prerequisite subject info not found";
        //                return dto;
        //            }
        //        }

        //        if (request.PrerequisiteConstraintId != null)
        //        {
        //            var prerequisiteConstraint = await _prerequisiteConstraintRepository.GetById(request.PrerequisiteConstraintId.Value);
        //            if (prerequisiteConstraint == null)
        //            {
        //                dto.IsSucess = false;
        //                dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
        //                dto.Message = "Prerequisite constraint not found";
        //                return dto;
        //            }
        //        }

        //        var existingPrerequisiteSubject = await _prerequisiteSubjectRepository.GetById(id);
        //        if (existingPrerequisiteSubject == null)
        //        {
        //            dto.IsSucess = false;
        //            dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
        //            dto.Message = "Prerequisite subject not found";
        //            return dto;
        //        }

        //        existingPrerequisiteSubject.RelationGroup = request.RelationGroup ?? existingPrerequisiteSubject.RelationGroup;
        //        existingPrerequisiteSubject.ConditionType = request.ConditionType ?? existingPrerequisiteSubject.ConditionType;
        //        existingPrerequisiteSubject.SubjectId = request.SubjectId ?? existingPrerequisiteSubject.SubjectId;
        //        existingPrerequisiteSubject.PrerequisiteSubjectId = request.PrerequisiteSubjectId ?? existingPrerequisiteSubject.PrerequisiteSubjectId;
        //        existingPrerequisiteSubject.PrerequisiteConstraintId = request.PrerequisiteConstraintId ?? existingPrerequisiteSubject.PrerequisiteConstraintId;
        //        existingPrerequisiteSubject.UpdatedAt = DateTime.Now;

        //        await _prerequisiteSubjectRepository.Update(existingPrerequisiteSubject);
        //        await _unitOfWork.SaveChangeAsync();

        //        dto.IsSucess = true;
        //        dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
        //        dto.Data = existingPrerequisiteSubject;
        //        dto.Message = "Prerequisite subject updated successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        dto.IsSucess = false;
        //        dto.BusinessCode = BusinessCode.EXCEPTION;
        //        dto.Message = "An error occurred while updating the prerequisite subject: " + ex.Message;
        //    }
        //    return dto;
        //}

        //public async Task<Response> DeletePrerequisiteSubject(Guid id)
        //{
        //    Response dto = new Response();
        //    try
        //    {
        //        var prerequisiteSubject = await _prerequisiteSubjectRepository.GetById(id);
        //        if (prerequisiteSubject == null)
        //        {
        //            dto.IsSucess = false;
        //            dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
        //            dto.Message = "Prerequisite subject not found";
        //            return dto;
        //        }

        //        prerequisiteSubject.IsDeleted = true;
        //        prerequisiteSubject.DeletedAt = DateTime.Now;

        //        await _prerequisiteSubjectRepository.Update(prerequisiteSubject);
        //        await _unitOfWork.SaveChangeAsync();

        //        dto.IsSucess = true;
        //        dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
        //        dto.Data = prerequisiteSubject;
        //        dto.Message = "Prerequisite subject deleted successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        dto.IsSucess = false;
        //        dto.BusinessCode = BusinessCode.EXCEPTION;
        //        dto.Message = "An error occurred while deleting the prerequisite subject: " + ex.Message;
        //    }
        //    return dto;
        //}

        public async Task<Response> GetAllPrerequisiteSubjectsForPrerequisiteConstrain(Guid prerequisiteConstrainId, int pageNumber, int pageSize)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _prerequisiteSubjectRepository.GetAllDataByExpression(
                    filter: x => x.PrerequisiteConstraintId == prerequisiteConstrainId,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: null,
                    isAscending: true,
                    includes: new Expression<Func<PrerequisiteSubject, object>>[]
                {
                    c => c.PrerequisiteSubjectInfo,
                    c => c.PrerequisiteConstraint
                });
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Prerequisite subject for Prerequisite constrain retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> AddPrerequisiteSubjectsToPrerequisiteConstrain(Guid prerequisiteConstrainId, List<PrerequisiteSubjectsToPrerequisiteConstrainRequest> requests)
        {
            Response dto = new Response();
            try
            {
                var prerequisiteConstraint = await _prerequisiteConstraintRepository.GetById(prerequisiteConstrainId);
                if (prerequisiteConstraint == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Prerequisite Constraint not found";
                    return dto;
                }

                var prerequisiteSubjectList = new List<PrerequisiteSubject>();
                foreach (var request in requests)
                {
                    var prerequisiteSubject = await _subjectRepository.GetById(request.PrerequisiteSubjectId);
                    if (prerequisiteSubject == null) continue;

                    var existing = await _prerequisiteSubjectRepository.GetFirstByExpression(
                        x => x.PrerequisiteConstraintId == prerequisiteConstrainId && x.PrerequisiteSubjectId == request.PrerequisiteSubjectId);
                    if (existing != null) continue;

                    prerequisiteSubjectList.Add(new PrerequisiteSubject
                    {
                        Id = Guid.NewGuid(),
                        PrerequisiteConstraintId = prerequisiteConstrainId,
                        PrerequisiteSubjectId = request.PrerequisiteSubjectId,
                        RelationGroup = request.RelationGroup,
                        CreatedAt = DateTime.Now,
                        ConditionType = request.ConditionType,
                    });
                }

                await _prerequisiteSubjectRepository.InsertRange(prerequisiteSubjectList);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = prerequisiteSubjectList;
                dto.Message = "Prerequisite Subjects added to prerequisite Constrain successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeletePrerequisiteSubjectsFromPrerequisiteConstrain(Guid prerequisiteConstrainId, List<Guid> prerequisiteSubjectIds)
        {
            var dto = new Response();
            try
            {
                var prerequisiteConstraint = await _prerequisiteConstraintRepository.GetById(prerequisiteConstrainId);
                if (prerequisiteConstraint == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Prerequisite Constrain not found";
                    return dto;
                }

                var prerequisiteSubjects = await _prerequisiteSubjectRepository.GetAllDataByExpression(
                    filter: x => x.PrerequisiteConstraintId == prerequisiteConstrainId && prerequisiteSubjectIds.Contains((Guid)x.PrerequisiteSubjectId),
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);

                if (prerequisiteSubjects == null || !prerequisiteSubjects.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No record found to delete";
                    return dto;
                }

                await _prerequisiteSubjectRepository.DeleteRange(prerequisiteSubjects.Items);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = prerequisiteSubjects.Items.Count;
                dto.Message = "Prerequisite Subjects deleted from prerequisite Constrain successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteAllPrerequisiteSubjectsFromPrerequisiteConstrain(Guid prerequisiteConstrainId)
        {
            var dto = new Response();
            try
            {
                var prerequisiteConstraint = await _prerequisiteConstraintRepository.GetById(prerequisiteConstrainId);
                if (prerequisiteConstraint == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Prerequisite Constrain not found";
                    return dto;
                }

                var prerequisiteSubjects = await _prerequisiteSubjectRepository.GetAllDataByExpression(
                    filter: x => x.PrerequisiteConstraintId == prerequisiteConstrainId,
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);

                if (prerequisiteSubjects == null || !prerequisiteSubjects.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No prerequisite Subjects found in prerequisite Constrain";
                    return dto;
                }

                await _prerequisiteSubjectRepository.DeleteRange(prerequisiteSubjects.Items);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Message = "All prerequisite Subjects deleted from prerequisite Constrain successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetReadablePrerequisiteExpression(Guid prerequisiteConstrainId)
        {
            Response dto = new Response();

            try
            {
                // Lấy ràng buộc + danh sách môn tiên quyết
                var constraint = await _prerequisiteConstraintRepository.GetById(prerequisiteConstrainId);
                if (constraint == null)
                {
                    dto.IsSucess = false;
                    dto.Message = "Prerequisite constraint not found.";
                    return dto;
                }

                var prerequisites = await _prerequisiteSubjectRepository.GetAllDataByExpression(
                    filter: x => x.PrerequisiteConstraintId == prerequisiteConstrainId,
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: x => x.PrerequisiteSubjectInfo);

                // Nhóm theo RelationGroup
                var grouped = prerequisites.Items
                    .GroupBy(x => x.RelationGroup)
                    .OrderBy(g => g.Key)
                    .ToList();

                List<string> groupExpressions = new();

                foreach (var group in grouped)
                {
                    var groupItems = group.Select(x => x.PrerequisiteSubjectInfo?.SubjectCode ?? "UNKNOWN").ToList();
                    var conditionType = group.First().ConditionType.ToString(); // AND / OR

                    var expression = string.Join($" {conditionType} ", groupItems);
                    if (groupItems.Count > 1)
                    {
                        expression = $"({expression})";
                    }
                    groupExpressions.Add(expression);
                }

                var finalExpression = string.Join($" {constraint.GroupCombinationType} ", groupExpressions);

                dto.Data = finalExpression;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Prerequisite expression retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred: " + ex.Message;
            }

            return dto;
        }

        public async Task<Response> GetReadablePrerequisiteExpressionOfSubjectInCurriculum(Guid subjectId, Guid curriculumId)
        {
            Response dto = new Response();

            try
            {
                // Lấy ràng buộc + danh sách môn tiên quyết
                var constraint = await _prerequisiteConstraintRepository.GetFirstByExpression(
                    filter: x => x.SubjectId == subjectId && x.CurriculumId == curriculumId);
                if (constraint == null)
                {
                    dto.IsSucess = false;
                    dto.Message = "Prerequisite constraint not found.";
                    return dto;
                }

                var prerequisites = await _prerequisiteSubjectRepository.GetAllDataByExpression(
                    filter: x => x.PrerequisiteConstraintId == constraint.PrerequisiteConstraintId,
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: x => x.PrerequisiteSubjectInfo);

                // Nhóm theo RelationGroup
                var grouped = prerequisites.Items
                    .GroupBy(x => x.RelationGroup)
                    .OrderBy(g => g.Key)
                    .ToList();

                List<string> groupExpressions = new();

                foreach (var group in grouped)
                {
                    var groupItems = group.Select(x => x.PrerequisiteSubjectInfo?.SubjectCode ?? "UNKNOWN").ToList();
                    var conditionType = group.First().ConditionType.ToString(); // AND / OR

                    var expression = string.Join($" {conditionType} ", groupItems);
                    if (groupItems.Count > 1)
                    {
                        expression = $"({expression})";
                    }
                    groupExpressions.Add(expression);
                }

                var finalExpression = string.Join($" {constraint.GroupCombinationType} ", groupExpressions);

                dto.Data = finalExpression;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Prerequisite expression retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred: " + ex.Message;
            }

            return dto;
        }

    }
}
