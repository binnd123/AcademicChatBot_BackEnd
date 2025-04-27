using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.ProgramingLearningOutcome;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Implementation
{
    public class ProgramingLearningOutcomeService : IProgramingLearningOutcomeService
    {
        private readonly IGenericRepository<ProgramingLearningOutcome> _programingLearningOutcomeRepository;
        private readonly IGenericRepository<Curriculum> _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProgramingLearningOutcomeService(IGenericRepository<ProgramingLearningOutcome> programingLearningOutcomeRepository, IGenericRepository<Curriculum> curriculumRepository, IUnitOfWork unitOfWork)
        {
            _programingLearningOutcomeRepository = programingLearningOutcomeRepository;
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateProgramingLearningOutcome(CreateProgramingLearningOutcomeRequest request)
        {
            Response dto = new Response();
            try
            {
                var curriculum = await _curriculumRepository.GetById(request.CurriculumId);
                if (curriculum == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Curriculum not found";
                    return dto;
                }

                var programingLearningOutcome = new ProgramingLearningOutcome
                {
                    ProgramingLearningOutcomeId = Guid.NewGuid(),
                    ProgramingLearningOutcomeCode = request.ProgramingLearningOutcomeCode,
                    ProgramingLearningOutcomeName = request.ProgramingLearningOutcomeName,
                    Description = request.Description,
                    CurriculumId = request.CurriculumId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                await _programingLearningOutcomeRepository.Insert(programingLearningOutcome);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = programingLearningOutcome;
                dto.Message = "Programing Learning Outcome created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating the programing learning outcome: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteProgramingLearningOutcome(Guid programingLearningOutcomeId)
        {
            Response dto = new Response();
            try
            {
                var programingLearningOutcome = await _programingLearningOutcomeRepository.GetById(programingLearningOutcomeId);
                if (programingLearningOutcome == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Programing Learning Outcome not found";
                    return dto;
                }
                programingLearningOutcome.IsDeleted = true;
                programingLearningOutcome.DeletedAt = DateTime.Now;
                await _programingLearningOutcomeRepository.Update(programingLearningOutcome);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = programingLearningOutcome;
                dto.Message = "Programing Learning Outcome deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting the programing learning outcome: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllProgramingLearningOutcomes(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDelete)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _programingLearningOutcomeRepository.GetAllDataByExpression(
                    filter: p => (p.ProgramingLearningOutcomeName.ToLower().Contains(search.ToLower()) || p.ProgramingLearningOutcomeCode.ToLower().Contains(search.ToLower()))
                    && p.IsDeleted == isDelete,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: p => sortBy == SortBy.Default ? null : sortBy == SortBy.Name ? p.ProgramingLearningOutcomeName : p.ProgramingLearningOutcomeCode,
                    isAscending: sortType == SortType.Ascending,
                    includes: p => p.Curriculum);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "ProgramingLearningOutcomes retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving programing learning outcomes: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetProgramingLearningOutcomeById(Guid programingLearningOutcomeId)
        {
            Response dto = new Response();
            try
            {
                var programingLearningOutcome = await _programingLearningOutcomeRepository.GetById(programingLearningOutcomeId);
                if (programingLearningOutcome == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "ProgramingLearningOutcome not found";
                    return dto;
                }
                dto.Data = programingLearningOutcome;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "ProgramingLearningOutcome retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the programing learning outcome: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> UpdateProgramingLearningOutcome(Guid programingLearningOutcomeId, UpdateProgramingLearningOutcomeRequest request)
        {
            Response dto = new Response();
            try
            {
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

                var programingLearningOutcome = await _programingLearningOutcomeRepository.GetById(programingLearningOutcomeId);
                if (programingLearningOutcome == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "ProgramingLearningOutcome not found";
                    return dto;
                }

                if (!string.IsNullOrEmpty(request.ProgramingLearningOutcomeCode)) programingLearningOutcome.ProgramingLearningOutcomeCode = request.ProgramingLearningOutcomeCode;
                if (!string.IsNullOrEmpty(request.ProgramingLearningOutcomeName)) programingLearningOutcome.ProgramingLearningOutcomeName = request.ProgramingLearningOutcomeName;
                if (!string.IsNullOrEmpty(request.Description)) programingLearningOutcome.Description = request.Description;
                programingLearningOutcome.CurriculumId = request.CurriculumId ?? programingLearningOutcome.CurriculumId;
                programingLearningOutcome.UpdatedAt = DateTime.Now;

                await _programingLearningOutcomeRepository.Update(programingLearningOutcome);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = programingLearningOutcome;
                dto.Message = "ProgramingLearningOutcome updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the programing learning outcome: " + ex.Message;
            }
            return dto;
        }
    }
}
