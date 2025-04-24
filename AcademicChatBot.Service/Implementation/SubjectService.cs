using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Subjects;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Implementation;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using Azure.Core;

namespace AcademicChatBot.Service.Implementation
{
    public class SubjectService : ISubjectService
    {
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IGenericRepository<Curriculum> _curriculumRepository;
        private readonly IGenericRepository<ToolForSubject> _toolForSubjectRepository;
        private readonly IGenericRepository<Tool> _toolRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubjectService(IGenericRepository<Subject> subjectRepository, IGenericRepository<Curriculum> curriculumRepository, IGenericRepository<ToolForSubject> toolForSubjectRepository, IGenericRepository<Tool> toolRepository, IUnitOfWork unitOfWork)
        {
            _subjectRepository = subjectRepository;
            _curriculumRepository = curriculumRepository;
            _toolForSubjectRepository = toolForSubjectRepository;
            _toolRepository = toolRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateSubject(CreateSubjectRequest request)
        {
            Response dto = new Response();
            try
            {
                if (request.CurriculumId != null)
                {
                    var curriculum = await _curriculumRepository.GetById(request.CurriculumId);
                    if (curriculum == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Curriculum not found";
                        return dto;
                    }
                }
                var subject = new Subject
                {
                    SubjectId = Guid.NewGuid(),
                    SubjectCode = request.SubjectCode,
                    IsApproved = request.IsApproved,
                    IsDeleted = false,
                    ApprovedDate = request.ApprovedDate,
                    IsActive = request.IsActive,
                    DecisionNo = request.DecisionNo,
                    NoCredit = request.NoCredit,
                    SubjectName = request.SubjectName,
                    CurriculumId = request.CurriculumId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    DegreeLevel = request.DegreeLevel,
                    Description = request.Description,
                    SyllabusName = request.SyllabusName,
                    TimeAllocation = request.TimeAllocation,
                    StudentTasks = request.StudentTasks,
                    ScoringScale = request.ScoringScale,
                    MinAvgMarkToPass = request.MinAvgMarkToPass,
                    Note = request.Note,
                    SessionNo = request.SessionNo,
                };
                await _subjectRepository.Insert(subject);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = subject;
                dto.Message = "Subject created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating the subject: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteSubject(Guid SubjectId)
        {
            Response dto = new Response();
            try
            {
                var subject = await _subjectRepository.GetById(SubjectId);
                if (subject == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Subject not found";
                    return dto;
                }
                subject.IsDeleted = true;
                await _subjectRepository.Update(subject);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = subject;
                dto.Message = "Subject deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting the subject: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllSubjects(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDelete)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _subjectRepository.GetAllDataByExpression(
                    filter: s => (s.SubjectCode.ToLower().Contains(search.ToLower()) || s.SubjectName.ToLower().Contains(search.ToLower()))
                    && s.IsDeleted == isDelete, 
                    pageNumber: pageNumber, 
                    pageSize: pageSize, 
                    orderBy: s => sortBy == SortBy.Default ? null : sortBy == SortBy.Name ? s.SubjectName : s.SubjectCode, 
                    isAscending: sortType == SortType.Ascending,
                    includes: s => s.Curriculum);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Subjects retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving subjects: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetSubjectById(Guid subjectId)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _subjectRepository.GetById(subjectId);
                if (dto.Data == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Subject not found";
                    return dto;
                }
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Subject retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the subject: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> UpdateSubject(Guid SubjectId, UpdateSubjectRequest request)
        {
            Response dto = new Response();
            try
            {
                if (request.CurriculumId != null)
                {
                    var curriculum = await _curriculumRepository.GetById(request.CurriculumId);
                    if (curriculum == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Curriculum not found";
                        return dto;
                    }
                }
                var subject = await _subjectRepository.GetById(SubjectId);
                if (subject == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Subject not found";
                    return dto;
                }

                if (!string.IsNullOrEmpty(request.SubjectName)) subject.SubjectName = request.SubjectName;
                if (!string.IsNullOrEmpty(request.SubjectCode)) subject.SubjectCode = request.SubjectCode;
                if (!string.IsNullOrEmpty(request.DecisionNo)) subject.DecisionNo = request.DecisionNo;
                if (request.ApprovedDate != default) subject.ApprovedDate = request.ApprovedDate;

                subject.IsActive = request.IsActive;
                subject.IsApproved = request.IsApproved;

                if (request.NoCredit > 0) subject.NoCredit = request.NoCredit;
                if (request.SessionNo > 0) subject.SessionNo = request.SessionNo;
                if (!string.IsNullOrEmpty(request.SyllabusName)) subject.SyllabusName = request.SyllabusName;
                if (!string.IsNullOrEmpty(request.DegreeLevel)) subject.DegreeLevel = request.DegreeLevel;
                if (!string.IsNullOrEmpty(request.TimeAllocation)) subject.TimeAllocation = request.TimeAllocation;
                if (!string.IsNullOrEmpty(request.Description)) subject.Description = request.Description;
                if (!string.IsNullOrEmpty(request.StudentTasks)) subject.StudentTasks = request.StudentTasks;
                if (request.ScoringScale > 0) subject.ScoringScale = request.ScoringScale;
                if (request.MinAvgMarkToPass > 0) subject.MinAvgMarkToPass = request.MinAvgMarkToPass;
                if (!string.IsNullOrEmpty(request.Note)) subject.Note = request.Note;
                if (request.CurriculumId != null) subject.CurriculumId = request.CurriculumId;

                subject.UpdatedAt = DateTime.Now;

                await _subjectRepository.Update(subject);

                await _toolForSubjectRepository.DeleteByExpression(filter: t => t.SubjectId == subject.SubjectId);

                // Tạo lại liên kết với ToolIds mới
                if (request.ToolIds != null && request.ToolIds.Any())
                {
                    foreach (var toolId in request.ToolIds)
                    {
                        var tool = await _toolRepository.GetById(toolId);
                        if (tool != null)
                        {
                            var toolForSubject = new ToolForSubject
                            {
                                ToolForSubjectId = Guid.NewGuid(),
                                SubjectId = subject.SubjectId,
                                ToolId = toolId
                            };
                            await _toolForSubjectRepository.Insert(toolForSubject);
                        }
                    }
                }

                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = subject;
                dto.Message = "Subject updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the subject: " + ex.Message;
            }
            return dto;
        }
    }
}
