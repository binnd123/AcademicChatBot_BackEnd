using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.CourseLearningOutcome;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;

namespace AcademicChatBot.Service.Implementation
{
    public class CourseLearningOutcomeService : ICourseLearningOutcomeService
    {
        private readonly IGenericRepository<CourseLearningOutcome> _courseLearningOutcomeRepository;
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IGenericRepository<Assessment> _assessmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CourseLearningOutcomeService(
            IGenericRepository<CourseLearningOutcome> courseLearningOutcomeRepository,
            IGenericRepository<Subject> subjectRepository,
            IGenericRepository<Assessment> assessmentRepository,
            IUnitOfWork unitOfWork)
        {
            _courseLearningOutcomeRepository = courseLearningOutcomeRepository;
            _subjectRepository = subjectRepository;
            _assessmentRepository = assessmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateCourseLearningOutcome(CreateCourseLearningOutcomeRequest request)
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

                if (request.AssessmentId != null)
                {
                    var assessment = await _assessmentRepository.GetById(request.AssessmentId.Value);
                    if (assessment == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Assessment not found";
                        return dto;
                    }
                }

                var clo = new CourseLearningOutcome
                {
                    CourseLearningOutcomeId = Guid.NewGuid(),
                    CourseLearningOutcomeCode = request.CourseLearningOutcomeCode,
                    CourseLearningOutcomeName = request.CourseLearningOutcomeName,
                    CourseLearningOutcomeDetail = request.CourseLearningOutcomeDetail,
                    SubjectId = request.SubjectId,
                    AssessmentId = request.AssessmentId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                await _courseLearningOutcomeRepository.Insert(clo);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = clo;
                dto.Message = "Course Learning Outcome created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating the course learning outcome: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllCourseLearningOutcomes(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDelete)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _courseLearningOutcomeRepository.GetAllDataByExpression(
                    filter: c => (c.CourseLearningOutcomeName.ToLower().Contains(search.ToLower()) || c.CourseLearningOutcomeCode.ToLower().Contains(search.ToLower())) && c.IsDeleted == isDelete,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: c => sortBy == SortBy.Default ? null : sortBy == SortBy.Name ? c.CourseLearningOutcomeName : c.CourseLearningOutcomeCode,
                    isAscending: sortType == SortType.Ascending,
                    includes: c => new { c.Subject, c.Assessment});

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Course Learning Outcomes retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving course learning outcomes: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetCourseLearningOutcomeById(Guid cloId)
        {
            Response dto = new Response();
            try
            {
                var clo = await _courseLearningOutcomeRepository.GetById(cloId);

                if (clo == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Course Learning Outcome not found";
                    return dto;
                }

                dto.Data = clo;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Course Learning Outcome retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the course learning outcome: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> UpdateCourseLearningOutcome(Guid cloId, UpdateCourseLearningOutcomeRequest request)
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

                if (request.AssessmentId != null)
                {
                    var assessment = await _assessmentRepository.GetById(request.AssessmentId.Value);
                    if (assessment == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Assessment not found";
                        return dto;
                    }
                }

                var clo = await _courseLearningOutcomeRepository.GetById(cloId);
                if (clo == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Course Learning Outcome not found";
                    return dto;
                }

                clo.CourseLearningOutcomeCode = request.CourseLearningOutcomeCode ?? clo.CourseLearningOutcomeCode;
                clo.CourseLearningOutcomeName = request.CourseLearningOutcomeName ?? clo.CourseLearningOutcomeName;
                clo.CourseLearningOutcomeDetail = request.CourseLearningOutcomeDetail ?? clo.CourseLearningOutcomeDetail;
                clo.SubjectId = request.SubjectId ?? clo.SubjectId;
                clo.AssessmentId = request.AssessmentId ?? clo.AssessmentId;
                clo.UpdatedAt = DateTime.UtcNow;

                await _courseLearningOutcomeRepository.Update(clo);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = clo;
                dto.Message = "Course Learning Outcome updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the course learning outcome: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteCourseLearningOutcome(Guid cloId)
        {
            Response dto = new Response();
            try
            {
                var clo = await _courseLearningOutcomeRepository.GetById(cloId);
                if (clo == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Course Learning Outcome not found";
                    return dto;
                }

                clo.IsDeleted = true;
                clo.DeletedAt = DateTime.UtcNow;

                await _courseLearningOutcomeRepository.Update(clo);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = clo;
                dto.Message = "Course Learning Outcome deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting the course learning outcome: " + ex.Message;
            }
            return dto;
        }
    }
}
