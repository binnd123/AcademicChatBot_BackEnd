using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.Assessment;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Implementation
{
    public class AssessmentService : IAssessmentService
    {
        private readonly IGenericRepository<Assessment> _assessmentRepository;
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssessmentService(IGenericRepository<Assessment> assessmentRepository, IGenericRepository<Subject> subjectRepository, IUnitOfWork unitOfWork)
        {
            _assessmentRepository = assessmentRepository;
            _subjectRepository = subjectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateAssessment(CreateAssessmentRequest request)
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

                var assessment = new Assessment
                {
                    AssessmentId = Guid.NewGuid(),
                    Category = request.Category,
                    Type = request.Type,
                    Part = request.Part,
                    Weight = request.Weight,
                    CompletionCriteria = request.CompletionCriteria,
                    Duration = request.Duration,
                    QuestionType = request.QuestionType,
                    NoQuestion = request.NoQuestion,
                    KnowledgeAndSkill = request.KnowledgeAndSkill,
                    GradingGuide = request.GradingGuide,
                    Note = request.Note,
                    SubjectId = request.SubjectId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _assessmentRepository.Insert(assessment);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = assessment;
                dto.Message = "Assessment created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating the assessment: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteAssessment(Guid assessmentId)
        {
            Response dto = new Response();
            try
            {
                var assessment = await _assessmentRepository.GetById(assessmentId);
                if (assessment == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Assessment not found";
                    return dto;
                }
                assessment.IsDeleted = true;
                assessment.DeletedAt = DateTime.UtcNow;
                await _assessmentRepository.Update(assessment);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = assessment;
                dto.Message = "Assessment deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting the assessment: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllAssessments(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _assessmentRepository.GetAllDataByExpression(
                    filter: a => a.Category.ToLower().Contains(search.ToLower()),
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: a => sortBy == SortBy.Default ? null : a.Category,
                    isAscending: sortType == SortType.Ascending,
                    includes: a => a.Subject);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Assessments retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving assessments: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAssessmentById(Guid assessmentId)
        {
            Response dto = new Response();
            try
            {
                var assessment = await _assessmentRepository.GetById(assessmentId);
                if (assessment == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Assessment not found";
                    return dto;
                }
                dto.Data = assessment;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Assessment retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the assessment: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> UpdateAssessment(Guid assessmentId, UpdateAssessmentRequest request)
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

                var assessment = await _assessmentRepository.GetById(assessmentId);
                if (assessment == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Assessment not found";
                    return dto;
                }

                assessment.Category = request.Category ?? assessment.Category;
                assessment.Type = request.Type ?? assessment.Type;
                assessment.Part = request.Part ?? assessment.Part;
                assessment.Weight = request.Weight ?? assessment.Weight;
                assessment.CompletionCriteria = request.CompletionCriteria ?? assessment.CompletionCriteria;
                assessment.Duration = request.Duration ?? assessment.Duration;
                assessment.QuestionType = request.QuestionType ?? assessment.QuestionType;
                assessment.NoQuestion = request.NoQuestion ?? assessment.NoQuestion;
                assessment.KnowledgeAndSkill = request.KnowledgeAndSkill ?? assessment.KnowledgeAndSkill;
                assessment.GradingGuide = request.GradingGuide ?? assessment.GradingGuide;
                assessment.Note = request.Note ?? assessment.Note;
                assessment.SubjectId = request.SubjectId ?? assessment.SubjectId;
                assessment.UpdatedAt = DateTime.UtcNow;

                await _assessmentRepository.Update(assessment);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = assessment;
                dto.Message = "Assessment updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the assessment: " + ex.Message;
            }
            return dto;
        }
    }
}
