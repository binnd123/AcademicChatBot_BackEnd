using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Tools;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;

namespace AcademicChatBot.Service.Implementation
{
    public class ToolService : IToolService
    {
        private readonly IGenericRepository<Tool> _toolRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ToolService(IGenericRepository<Tool> toolRepository, IUnitOfWork unitOfWork)
        {
            _toolRepository = toolRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> CreateTool(CreateToolRequest request)
        {
            Response dto = new Response();
            try
            {
                var toolE = await _toolRepository.GetFirstByExpression(x => x.ToolCode == request.ToolCode);
                if (toolE != null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.EXCEPTION;
                    dto.Message = "Tool is Existed!";
                    return dto;
                }

                var tool = new Tool
                {
                    ToolId = Guid.NewGuid(),
                    ToolCode = request.ToolCode,
                    ToolName = request.ToolName,
                    Description = request.Description,
                    Author = request.Author,
                    Publisher = request.Publisher,
                    PublishedDate = request.PublishedDate,
                    Note = request.Note,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                await _toolRepository.Insert(tool);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = tool;
                dto.Message = "Tool created successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while creating the tool: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteTool(Guid toolId)
        {
            Response dto = new Response();
            try
            {
                var tool = await _toolRepository.GetById(toolId);
                if (tool == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Tool not found";
                    return dto;
                }
                tool.IsDeleted = true;
                tool.DeletedAt = DateTime.Now;
                await _toolRepository.Update(tool);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = tool;
                dto.Message = "Tool deleted successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting the tool: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllTools(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDeleted)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _toolRepository.GetAllDataByExpression(
                    filter: t => (t.ToolName.ToLower().Contains(search.ToLower()) || t.ToolCode.ToLower().Contains(search.ToLower()))
                    && t.IsDeleted == isDeleted,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: t => sortBy == SortBy.Default ? null : sortBy == SortBy.Name ? t.ToolName : t.ToolCode,
                    isAscending: sortType == SortType.Ascending);
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Tools retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving tools: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetToolById(Guid toolId)
        {
            Response dto = new Response();
            try
            {
                var tool = await _toolRepository.GetById(toolId);
                if (tool == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Tool not found";
                    return dto;
                }
                dto.Data = tool;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Tool retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving the tool: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> UpdateTool(Guid toolId, UpdateToolRequest request)
        {
            Response dto = new Response();
            try
            {
                var tool = await _toolRepository.GetById(toolId);
                if (tool == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Tool not found";
                    return dto;
                }

                if (!string.IsNullOrEmpty(request.ToolCode)) tool.ToolCode = request.ToolCode;
                if (!string.IsNullOrEmpty(request.ToolName)) tool.ToolName = request.ToolName;
                if (!string.IsNullOrEmpty(request.Description)) tool.Description = request.Description;
                if (!string.IsNullOrEmpty(request.Author)) tool.Author = request.Author;
                if (!string.IsNullOrEmpty(request.Publisher)) tool.Publisher = request.Publisher;
                if (!string.IsNullOrEmpty(request.Note)) tool.Note = request.Note;
                tool.PublishedDate = request.PublishedDate ?? tool.PublishedDate;
                tool.UpdatedAt = DateTime.Now;

                await _toolRepository.Update(tool);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.UPDATE_SUCCESSFULLY;
                dto.Data = tool;
                dto.Message = "Tool updated successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while updating the tool: " + ex.Message;
            }
            return dto;
        }
    }
}
