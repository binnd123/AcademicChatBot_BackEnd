using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.ProgramingOutcome;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Implementation
{
    public class ProgramingOutcomeService : IProgramingOutcomeService
    {
        private readonly IGenericRepository<ProgramingOutcome> _programingOutcomeRepository;
        private readonly IGenericRepository<Program> _programRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProgramingOutcomeService(
            IGenericRepository<ProgramingOutcome> programingOutcomeRepository,
            IGenericRepository<Program> programRepository,
            IUnitOfWork unitOfWork)
        {
            _programingOutcomeRepository = programingOutcomeRepository;
            _programRepository = programRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateProgramingOutcome(CreateProgramingOutcomeRequest request)
        {
            Response dto = new Response();
            try
            {
                var program = await _programRepository.GetById(request.ProgramId);
                if (program == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Program not found";
                    return dto;
                }

                var programingOutcome = new ProgramingOutcome
                {
                    ProgramingOutcomeId = Guid.NewGuid(),
                    ProgramingOutcomeCode = request.ProgramingOutcomeCode,
                    ProgramingOutcomeName = request.ProgramingOutcomeName,
                    Description = request.Description,
                    ProgramId = request.ProgramId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false
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
                programingOutcome.DeletedAt = DateTime.Now;
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

        public async Task<Response> GetAllProgramingOutcomes(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDelete)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _programingOutcomeRepository.GetAllDataByExpression(
                    filter: p => (p.ProgramingOutcomeName.ToLower().Contains(search.ToLower()) || p.ProgramingOutcomeCode.ToLower().Contains(search.ToLower()))
                    && p.IsDeleted == isDelete,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: s => sortBy == SortBy.Default ? null : sortBy == SortBy.Name ? s.ProgramingOutcomeName : s.ProgramingOutcomeCode,
                    isAscending: sortType == SortType.Ascending,
                    includes: p => p.Program);
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
                if (request.ProgramId != null)
                {
                    var program = await _programRepository.GetById(request.ProgramId.Value);
                    if (program == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Program not found";
                        return dto;
                    }
                }

                var programingOutcome = await _programingOutcomeRepository.GetById(programingOutcomeId);
                if (programingOutcome == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "ProgramingOutcome not found";
                    return dto;
                }

                if (!string.IsNullOrEmpty(request.ProgramingOutcomeCode)) programingOutcome.ProgramingOutcomeCode = request.ProgramingOutcomeCode;
                if (!string.IsNullOrEmpty(request.ProgramingOutcomeName)) programingOutcome.ProgramingOutcomeName = request.ProgramingOutcomeName;
                if (!string.IsNullOrEmpty(request.Description)) programingOutcome.Description = request.Description;
                programingOutcome.ProgramId = request.ProgramId ?? programingOutcome.ProgramId;
                programingOutcome.UpdatedAt = DateTime.Now;

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
