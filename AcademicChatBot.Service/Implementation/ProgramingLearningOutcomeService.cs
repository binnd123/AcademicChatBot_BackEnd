using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.ProgramingLearningOutcome;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;

namespace AcademicChatBot.Service.Implementation
{
    public class ProgramingLearningOutcomeService : IProgramingLearningOutcomeService
    {
        private readonly IGenericRepository<ProgramingLearningOutcome> _programingLearningOutcomeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProgramingLearningOutcomeService(IGenericRepository<ProgramingLearningOutcome> programingLearningOutcomeRepository, IUnitOfWork unitOfWork)
        {
            _programingLearningOutcomeRepository = programingLearningOutcomeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateProgramingLearningOutcome(CreateProgramingLearningOutcomeRequest request)
        {
            Response dto = new Response();
            try
            {
                var programingLearningOutcome = new ProgramingLearningOutcome
                {
                    ProgramingLearningOutcomeId = Guid.NewGuid(),
                    ProgramingLearningOutcomeCode = request.ProgramingLearningOutcomeCode,
                    ProgramingLearningOutcomeName = request.ProgramingLearningOutcomeName,
                    Description = request.Description,
                    CurriculumId = request.CurriculumId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _programingLearningOutcomeRepository.Insert(programingLearningOutcome);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = programingLearningOutcome;
                dto.Message = "ProgramingLearningOutcome created successfully";
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
                    dto.Message = "ProgramingLearningOutcome not found";
                    return dto;
                }
                programingLearningOutcome.IsDeleted = true;
                programingLearningOutcome.DeletedAt = DateTime.UtcNow;
                await _programingLearningOutcomeRepository.Update(programingLearningOutcome);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = programingLearningOutcome;
                dto.Message = "ProgramingLearningOutcome deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting the programing learning outcome: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllProgramingLearningOutcomes(int pageNumber, int pageSize, string search)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _programingLearningOutcomeRepository.GetAllDataByExpression(
                    filter: p => p.ProgramingLearningOutcomeName.ToLower().Contains(search.ToLower()) && !p.IsDeleted,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: p => p.ProgramingLearningOutcomeName,
                    isAscending: true);
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
                var programingLearningOutcome = await _programingLearningOutcomeRepository.GetById(programingLearningOutcomeId);
                if (programingLearningOutcome == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "ProgramingLearningOutcome not found";
                    return dto;
                }

                programingLearningOutcome.ProgramingLearningOutcomeCode = request.ProgramingLearningOutcomeCode ?? programingLearningOutcome.ProgramingLearningOutcomeCode;
                programingLearningOutcome.ProgramingLearningOutcomeName = request.ProgramingLearningOutcomeName ?? programingLearningOutcome.ProgramingLearningOutcomeName;
                programingLearningOutcome.Description = request.Description ?? programingLearningOutcome.Description;
                programingLearningOutcome.CurriculumId = request.CurriculumId ?? programingLearningOutcome.CurriculumId;
                programingLearningOutcome.UpdatedAt = DateTime.UtcNow;

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
