using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Programs;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.BussinessModel.Programs;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;

namespace AcademicChatBot.Service.Implementation
{
    public class ProgramService : IProgramService
    {
        private readonly IGenericRepository<Program> _programRepository;
        private readonly IGenericRepository<Combo> _comboRepository;
        private readonly IGenericRepository<ProgramingOutcome> _programingOutcomeRepository;
        private readonly IGenericRepository<Curriculum> _curriculumRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProgramService(IGenericRepository<Program> programRepository, IGenericRepository<Combo> comboRepository, IGenericRepository<ProgramingOutcome> programingOutcomeRepository, IGenericRepository<Curriculum> curriculumRepository, IUnitOfWork unitOfWork)
        {
            _programRepository = programRepository;
            _comboRepository = comboRepository;
            _programingOutcomeRepository = programingOutcomeRepository;
            _curriculumRepository = curriculumRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateProgram(CreateProgramRequest request)
        {
            Response dto = new Response();
            try
            {
                var programE = await _programRepository.GetFirstByExpression(x => x.ProgramCode == request.ProgramCode);
                if (programE != null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXCEPTION;
                    dto.Message = "Programing is Existed!";
                    return dto;
                }

                var program = new Program
                {
                    ProgramId = Guid.NewGuid(),
                    ProgramCode = request.ProgramCode,
                    ProgramName = request.ProgramName,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    DeletedAt = null,
                    IsDeleted = false,
                    StartAt = request.StartAt,
                };
                await _programRepository.Insert(program);

                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = program;
                dto.Message = "Program created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating the program: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteProgram(Guid programId)
        {
            Response dto = new Response();
            try
            {
                var program = await _programRepository.GetById(programId);
                if (program == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Program not found";
                    return dto;
                }
                program.IsDeleted = true;
                program.DeletedAt = DateTime.Now;
                await _programRepository.Update(program);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = program;
                dto.Message = "Program deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting the program: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllPrograms(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDeleted)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _programRepository.GetAllDataByExpression(
                    filter: s => (s.ProgramCode.ToLower().Contains(search) || s.ProgramName.ToLower().Contains(search))
                    && s.IsDeleted == isDeleted,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: s => sortBy == SortBy.Default ? null : sortBy == SortBy.Name ? s.ProgramName : s.ProgramCode,
                    isAscending: sortType == SortType.Ascending,
                    includes: null);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Program retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving Programs: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetProgramById(Guid programId)
        {
            Response dto = new Response();
            try
            {
                var program = await _programRepository.GetById(programId);
                if (program == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Program not found";
                    return dto;
                }
                var combos = await _comboRepository.GetAllDataByExpression(
                    filter: s => s.ProgramId == programId && s.IsApproved && s.IsActive && !s.IsDeleted,
                    pageNumber: 1,
                    pageSize: 1000,
                    orderBy: null,
                    isAscending: true,
                    includes: null);
                var pos = await _programingOutcomeRepository.GetAllDataByExpression(
                    filter: s => s.ProgramId == programId && !s.IsDeleted,
                    pageNumber: 1,
                    pageSize: 1000,
                    orderBy: null,
                    isAscending: true,
                    includes: null);
                var curriculums = await _curriculumRepository.GetAllDataByExpression(
                    filter: s => s.ProgramId == programId && s.IsApproved && s.IsActive && !s.IsDeleted,
                    pageNumber: 1,
                    pageSize: 1000,
                    orderBy: null,
                    isAscending: true,
                    includes: null);
                dto.Data = new DetailProgramResponse
                {
                    ProgramId = program.ProgramId,
                    ProgramCode = program.ProgramCode,
                    ProgramName = program.ProgramName,
                    CreatedAt = program.CreatedAt,
                    UpdatedAt = program.UpdatedAt,
                    DeletedAt = program.DeletedAt,
                    StartAt = program.StartAt,
                    IsDeleted = program.IsDeleted,
                    Combos = combos.Items,
                    Curriculums = curriculums.Items,
                    ProgramingOutcomes = pos.Items
                };
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Program retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the Program: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> UpdateProgram(Guid programId, UpdateProgramRequest request)
        {
            Response dto = new Response();
            try
            {
                var program = await _programRepository.GetById(programId);
                if (program == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Program not found";
                    return dto;
                }

                if (!string.IsNullOrEmpty(request.ProgramName)) program.ProgramName = request.ProgramName;
                if (!string.IsNullOrEmpty(request.ProgramCode)) program.ProgramCode = request.ProgramCode;
                program.UpdatedAt = DateTime.Now;
                program.StartAt = request.StartAt;

                await _programRepository.Update(program);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = program;
                dto.Message = "Program updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the Program: " + ex.Message;
            }
            return dto;
        }
    }
}
