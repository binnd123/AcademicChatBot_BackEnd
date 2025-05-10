using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Subjects;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.BussinessModel.Subjects;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using Azure.Core;

namespace AcademicChatBot.Service.Implementation
{
    public class SubjectService : ISubjectService
    {
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IGenericRepository<Assessment> _asessmentRepository;
        private readonly IGenericRepository<CourseLearningOutcome> _courseLearningOutcomeRepository;
        private readonly IGenericRepository<ToolForSubject> _toolForSubjectRepository;
        private readonly IGenericRepository<Material> _materialRepository;
        private readonly IPrerequisiteSubjectService _prerequisiteSubjectService;
        private readonly IUnitOfWork _unitOfWork;

        public SubjectService(IGenericRepository<Subject> subjectRepository, IGenericRepository<Assessment> asessmentRepository, IGenericRepository<CourseLearningOutcome> courseLearningOutcomeRepository, IGenericRepository<ToolForSubject> toolForSubjectRepository, IGenericRepository<Material> materialRepository, IPrerequisiteSubjectService prerequisiteSubjectService, IUnitOfWork unitOfWork)
        {
            _subjectRepository = subjectRepository;
            _asessmentRepository = asessmentRepository;
            _courseLearningOutcomeRepository = courseLearningOutcomeRepository;
            _toolForSubjectRepository = toolForSubjectRepository;
            _materialRepository = materialRepository;
            _prerequisiteSubjectService = prerequisiteSubjectService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateSubject(CreateSubjectRequest request)
        {
            Response dto = new Response();
            try
            {
                var subjectE = await _subjectRepository.GetFirstByExpression(x => x.SubjectCode == request.SubjectCode);
                if (subjectE != null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXCEPTION;
                    dto.Message = "Subject is Existed!";
                    return dto;
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
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
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

        public async Task<Response> GetAllSubjects(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDeleted)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _subjectRepository.GetAllDataByExpression(
                    filter: s => (s.SubjectCode.ToLower().Contains(search.ToLower()) || s.SubjectName.ToLower().Contains(search.ToLower()))
                    && s.IsDeleted == isDeleted, 
                    pageNumber: pageNumber, 
                    pageSize: pageSize, 
                    orderBy: s => sortBy == SortBy.Default ? null : sortBy == SortBy.Name ? s.SubjectName : s.SubjectCode, 
                    isAscending: sortType == SortType.Ascending,
                    includes: null);
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
                var subject = await _subjectRepository.GetById(subjectId);
                if (subject == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Subject not found";
                    return dto;
                }
                var assessments = await _asessmentRepository.GetAllDataByExpression(
                    filter: s => s.SubjectId == subjectId && !s.IsDeleted,
                    pageNumber: 1,
                    pageSize: 1000,
                    orderBy: null,
                    isAscending: true,
                    includes: null);
                var clos = await _courseLearningOutcomeRepository.GetAllDataByExpression(
                    filter: s => s.SubjectId == subjectId && !s.IsDeleted,
                    pageNumber: 1,
                    pageSize: 1000,
                    orderBy: null,
                    isAscending: true,
                    includes: null);
                var tools = await _toolForSubjectRepository.GetAllDataByExpression(
                    filter: s => s.SubjectId == subjectId,
                    pageNumber: 1,
                    pageSize: 1000,
                    orderBy: null,
                    isAscending: true,
                    includes: t => t.Tool);
                var materials = await _materialRepository.GetAllDataByExpression(
                    filter: s => s.SubjectId == subjectId && !s.IsDeleted,
                    pageNumber: 1,
                    pageSize: 1000,
                    orderBy: null,
                    isAscending: true,
                    includes: null);

                var prerequisiteSubjectsResponse = await _prerequisiteSubjectService.GetReadablePrerequisiteExpressionOfSubject(subjectId);
                var prerequisiteSubjects = (object)null;
                if (prerequisiteSubjectsResponse.IsSucess)
                {
                    prerequisiteSubjects = prerequisiteSubjectsResponse.Data;
                }
                else
                {
                    prerequisiteSubjects = null;
                }
                dto.Data = new DetailSubjectResponse
                {
                    SubjectId = subject.SubjectId,
                    SubjectCode = subject.SubjectCode,
                    SubjectName = subject.SubjectName,
                    SessionNo = subject.SessionNo,
                    DecisionNo = subject.DecisionNo,
                    IsActive = subject.IsActive,
                    IsApproved = subject.IsApproved,
                    CreatedAt = subject.CreatedAt,
                    UpdatedAt = subject.UpdatedAt,
                    DeletedAt = subject.DeletedAt,
                    IsDeleted = subject.IsDeleted,
                    NoCredit = subject.NoCredit,
                    ApprovedDate = subject.ApprovedDate,
                    SyllabusName = subject.SyllabusName,
                    DegreeLevel = subject.DegreeLevel,
                    TimeAllocation = subject.TimeAllocation,
                    Description = subject.Description,
                    StudentTasks = subject.StudentTasks,
                    ScoringScale = subject.ScoringScale,
                    MinAvgMarkToPass = subject.MinAvgMarkToPass,
                    Note = subject.Note,
                    Assessments = assessments.Items,
                    CourseLearningOutcomes = clos.Items,
                    Tools = tools.Items.Select(x => x.Tool).ToList(),
                    Materials = materials.Items,
                    PrerequisiteSubjects = prerequisiteSubjects
                };
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

                subject.UpdatedAt = DateTime.Now;

                await _subjectRepository.Update(subject);

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
