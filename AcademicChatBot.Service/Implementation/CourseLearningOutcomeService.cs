using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly IUnitOfWork _unitOfWork;

        public CourseLearningOutcomeService(
            IGenericRepository<CourseLearningOutcome> courseLearningOutcomeRepository,
            IGenericRepository<Subject> subjectRepository,
            IUnitOfWork unitOfWork)
        {
            _courseLearningOutcomeRepository = courseLearningOutcomeRepository;
            _subjectRepository = subjectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateCourseLearningOutcome(CreateCourseLearningOutcomeRequest request)
        {
            Response dto = new Response();
            try
            {
                var cloE = await _courseLearningOutcomeRepository.GetFirstByExpression(x => x.CourseLearningOutcomeCode == request.CourseLearningOutcomeCode);
                if (cloE != null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXCEPTION;
                    dto.Message = "Course Learning Outcome is Existed!";
                    return dto;
                }

                var subject = await _subjectRepository.GetById(request.SubjectId);
                if (subject == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Subject not found";
                    return dto;
                }

                var clo = new CourseLearningOutcome
                {
                    CourseLearningOutcomeId = Guid.NewGuid(),
                    CourseLearningOutcomeCode = request.CourseLearningOutcomeCode,
                    CourseLearningOutcomeName = request.CourseLearningOutcomeName,
                    CourseLearningOutcomeDetail = request.CourseLearningOutcomeDetail,
                    SubjectId = request.SubjectId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
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

        public async Task<Response> GetAllCourseLearningOutcomes(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDeleted)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _courseLearningOutcomeRepository.GetAllDataByExpression(
                    filter: c => (c.CourseLearningOutcomeName.ToLower().Contains(search.ToLower()) || c.CourseLearningOutcomeCode.ToLower().Contains(search.ToLower()))
                    && c.IsDeleted == isDeleted,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: c => sortBy == SortBy.Default ? null : sortBy == SortBy.Name ? c.CourseLearningOutcomeName : c.CourseLearningOutcomeCode,
                    isAscending: sortType == SortType.Ascending,
                    includes: c => c.Subject);

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

                var clo = await _courseLearningOutcomeRepository.GetById(cloId);
                if (clo == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Course Learning Outcome not found";
                    return dto;
                }

                if (!string.IsNullOrEmpty(request.CourseLearningOutcomeCode)) clo.CourseLearningOutcomeCode = request.CourseLearningOutcomeCode;
                if (!string.IsNullOrEmpty(request.CourseLearningOutcomeName)) clo.CourseLearningOutcomeName = request.CourseLearningOutcomeName;
                if (!string.IsNullOrEmpty(request.CourseLearningOutcomeDetail)) clo.CourseLearningOutcomeDetail = request.CourseLearningOutcomeDetail;
                clo.SubjectId = request.SubjectId ?? clo.SubjectId;
                clo.UpdatedAt = DateTime.Now;

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
                clo.DeletedAt = DateTime.Now;

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
