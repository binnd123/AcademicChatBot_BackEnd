using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.ProgramingOutcome;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;

namespace AcademicChatBot.Service.Implementation
{
    public class ProgramingOutcomeService : IProgramingOutcomeService
    {
        private readonly IGenericRepository<ProgramingOutcome> _programingOutcomeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProgramingOutcomeService(IGenericRepository<ProgramingOutcome> programingOutcomeRepository, IUnitOfWork unitOfWork)
        {
            _programingOutcomeRepository = programingOutcomeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateProgramingOutcome(CreateProgramingOutcomeRequest request)
        {
            Response dto = new Response();
            try
            {
                var programingOutcome = new ProgramingOutcome
                {
                    ProgramingOutcomeId = Guid.NewGuid(),
                    ProgramingOutcomeCode = request.ProgramingOutcomeCode,
                    ProgramingOutcomeName = request.ProgramingOutcomeName,
                    Description = request.Description,
                    ProgramId = request.ProgramId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _programingOutcomeRepository.Insert(programingOutcome);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = programingOutcome;
                dto.Message = "ProgramingOutcome created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating the programing outcome: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteProgramingOutcome(Guid programingOutcomeId)
        {
            Response dto = new Response();
            try
            {
                var programingOutcome = await _programingOutcomeRepository.GetById(programingOutcomeId);
                if (programingOutcome == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "ProgramingOutcome not found";
                    return dto;
                }
                programingOutcome.IsDeleted = true;
                programingOutcome.DeletedAt = DateTime.UtcNow;
                await _programingOutcomeRepository.Update(programingOutcome);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = programingOutcome;
                dto.Message = "ProgramingOutcome deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting the programing outcome: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllProgramingOutcomes(int pageNumber, int pageSize, string search)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _programingOutcomeRepository.GetAllDataByExpression(
                    filter: p => p.ProgramingOutcomeName.ToLower().Contains(search.ToLower()) && !p.IsDeleted,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: p => p.ProgramingOutcomeName,
                    isAscending: true);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "ProgramingOutcomes retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving programing outcomes: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetProgramingOutcomeById(Guid programingOutcomeId)
        {
            Response dto = new Response();
            try
            {
                var programingOutcome = await _programingOutcomeRepository.GetById(programingOutcomeId);
                if (programingOutcome == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "ProgramingOutcome not found";
                    return dto;
                }
                dto.Data = programingOutcome;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "ProgramingOutcome retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the programing outcome: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> UpdateProgramingOutcome(Guid programingOutcomeId, UpdateProgramingOutcomeRequest request)
        {
            Response dto = new Response();
            try
            {
                var programingOutcome = await _programingOutcomeRepository.GetById(programingOutcomeId);
                if (programingOutcome == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "ProgramingOutcome not found";
                    return dto;
                }

                programingOutcome.ProgramingOutcomeCode = request.ProgramingOutcomeCode ?? programingOutcome.ProgramingOutcomeCode;
                programingOutcome.ProgramingOutcomeName = request.ProgramingOutcomeName ?? programingOutcome.ProgramingOutcomeName;
                programingOutcome.Description = request.Description ?? programingOutcome.Description;
                programingOutcome.ProgramId = request.ProgramId ?? programingOutcome.ProgramId;
                programingOutcome.UpdatedAt = DateTime.UtcNow;

                await _programingOutcomeRepository.Update(programingOutcome);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = programingOutcome;
                dto.Message = "ProgramingOutcome updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the programing outcome: " + ex.Message;
            }
            return dto;
        }
    }
}
