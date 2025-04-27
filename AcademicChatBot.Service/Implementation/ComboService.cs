using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Combo;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;

namespace AcademicChatBot.Service.Implementation
{
    public class ComboService : IComboService
    {
        private readonly IGenericRepository<Combo> _comboRepository;
        private readonly IGenericRepository<Program> _programRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ComboService(IGenericRepository<Combo> comboRepository, IGenericRepository<Program> programRepository, IUnitOfWork unitOfWork)
        {
            _comboRepository = comboRepository;
            _programRepository = programRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateCombo(CreateComboRequest request)
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

                var combo = new Combo
                {
                    ComboId = Guid.NewGuid(),
                    ComboCode = request.ComboCode,
                    ComboName = request.ComboName,
                    Note = request.Note,
                    Description = request.Description,
                    IsActive = request.IsActive,
                    IsApproved = request.IsApproved,
                    ProgramId = request.ProgramId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsDeleted = false
                };

                await _comboRepository.Insert(combo);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = combo;
                dto.Message = "Combo created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating the combo: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllCombos(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDelete)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _comboRepository.GetAllDataByExpression(
                    filter: c => (c.ComboName.ToLower().Contains(search.ToLower()) || c.ComboCode.ToLower().Contains(search.ToLower())) && c.IsDeleted == isDelete,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: c => sortBy == SortBy.Default ? null : sortBy == SortBy.Name ? c.ComboName : c.ComboCode,
                    isAscending: sortType == SortType.Ascending,
                    includes: c => c.Program
                );

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Combos retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving combos: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetComboById(Guid id)
        {
            Response dto = new Response();
            try
            {
                var combo = await _comboRepository.GetById(id);

                if (combo == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Combo not found";
                    return dto;
                }

                dto.Data = combo;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Combo retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the combo: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> UpdateCombo(Guid id, UpdateComboRequest request)
        {
            Response dto = new Response();
            try
            {
                if (request.ProgramId != null)
                {
                    var program = await _programRepository.GetById(request.ProgramId);
                    if (program == null)
                    {
                        dto.IsSucess = false;
                        dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                        dto.Message = "Program not found";
                        return dto;
                    }
                }

                var combo = await _comboRepository.GetById(id);
                if (combo == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Combo not found";
                    return dto;
                }

                if (!string.IsNullOrEmpty(request.ComboCode)) combo.ComboCode = request.ComboCode;
                if (!string.IsNullOrEmpty(request.ComboName)) combo.ComboName = request.ComboName;
                if (!string.IsNullOrEmpty(request.Note)) combo.Note = request.Note;
                if (!string.IsNullOrEmpty(request.Description)) combo.Description = request.Description;
                combo.IsActive = request.IsActive;
                combo.IsApproved = request.IsApproved;
                combo.ProgramId = request.ProgramId ?? combo.ProgramId;
                combo.UpdatedAt = DateTime.Now;

                await _comboRepository.Update(combo);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = combo;
                dto.Message = "Combo updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the combo: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteCombo(Guid id)
        {
            Response dto = new Response();
            try
            {
                var combo = await _comboRepository.GetById(id);
                if (combo == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Combo not found";
                    return dto;
                }

                combo.IsDeleted = true;
                combo.DeletedAt = DateTime.Now;

                await _comboRepository.Update(combo);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = combo;
                dto.Message = "Combo deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting the combo: " + ex.Message;
            }
            return dto;
        }
    }
}
