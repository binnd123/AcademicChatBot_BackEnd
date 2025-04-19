using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.Tools;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.Tool;
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
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
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
                tool.DeletedAt = DateTime.UtcNow;
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

        public async Task<Response> GetAllTools(int pageNumber, int pageSize, string search)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _toolRepository.GetAllDataByExpression(
                    filter: t => t.ToolName.ToLower().Contains(search.ToLower()) && !t.IsDeleted,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: t => t.ToolName,
                    isAscending: true);
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

                tool.ToolCode = request.ToolCode ?? tool.ToolCode;
                tool.ToolName = request.ToolName ?? tool.ToolName;
                tool.Description = request.Description ?? tool.Description;
                tool.Author = request.Author ?? tool.Author;
                tool.Publisher = request.Publisher ?? tool.Publisher;
                tool.PublishedDate = request.PublishedDate ?? tool.PublishedDate;
                tool.Note = request.Note ?? tool.Note;
                tool.UpdatedAt = DateTime.UtcNow;

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
