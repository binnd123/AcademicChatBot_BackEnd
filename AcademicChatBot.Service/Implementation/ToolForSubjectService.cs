using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Contract;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using Azure.Core;

namespace AcademicChatBot.Service.Implementation
{
    public class ToolForSubjectService : IToolForSubjectService
    {
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IGenericRepository<Tool> _toolRepository;
        private readonly IGenericRepository<ToolForSubject> _toolForSubjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ToolForSubjectService(IGenericRepository<Subject> subjectRepository, IGenericRepository<Tool> toolRepository, IGenericRepository<ToolForSubject> toolForSubjectRepository, IUnitOfWork unitOfWork)
        {
            _subjectRepository = subjectRepository;
            _toolRepository = toolRepository;
            _toolForSubjectRepository = toolForSubjectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> AddSubjectsToTool(Guid toolId, List<Guid> subjectIds)
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
                var toolSubjectList = new List<ToolForSubject>();
                foreach (var subjectId in subjectIds)
                {
                    var subject = await _subjectRepository.GetById(subjectId);
                    if (subject == null) continue;
                    var existingToolForSubject = await _toolForSubjectRepository.GetFirstByExpression(x => x.ToolId == toolId && x.SubjectId == subjectId);
                    if (existingToolForSubject != null) continue;
                    toolSubjectList.Add(new ToolForSubject
                    {
                        ToolForSubjectId = Guid.NewGuid(),
                        ToolId = toolId,
                        SubjectId = subjectId,
                    });
                }
                await _toolForSubjectRepository.InsertRange(toolSubjectList);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.INSERT_SUCESSFULLY;
                dto.Data = toolSubjectList;
                dto.Message = "Add Subjects To Tool successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while Add Subjects To Tool: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> AddToolsToSubject(Guid subjectId, List<Guid> toolIds)
        {
            try
            {
                var subject = await _subjectRepository.GetById(subjectId);
                if (subject == null)
                {
                    return new Response
                    {
                        IsSucess = false,
                        BusinessCode = BusinessCode.DATA_NOT_FOUND,
                        Message = "Subject not found"
                    };
                }

                var toolSubjectList = new List<ToolForSubject>();
                foreach (var toolId in toolIds)
                {
                    var tool = await _toolRepository.GetById(toolId);
                    if (tool == null) continue;

                    var exists = await _toolForSubjectRepository
                        .GetFirstByExpression(x => x.SubjectId == subjectId && x.ToolId == toolId);
                    if (exists != null) continue;

                    toolSubjectList.Add(new ToolForSubject
                    {
                        ToolForSubjectId = Guid.NewGuid(),
                        ToolId = toolId,
                        SubjectId = subjectId,
                    });
                }

                await _toolForSubjectRepository.InsertRange(toolSubjectList);
                await _unitOfWork.SaveChangeAsync();

                return new Response
                {
                    IsSucess = true,
                    BusinessCode = BusinessCode.INSERT_SUCESSFULLY,
                    Message = "Add Tools To Subject successfully",
                    Data = toolSubjectList.Select(x => x.ToolId).ToList()
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSucess = false,
                    BusinessCode = BusinessCode.EXCEPTION,
                    Message = "An error occurred while Add Tools To Subject: " + ex.Message
                };
            }
        }

        public async Task<Response> DeleteAllSubjectsFromTool(Guid toolId)
        {
            var dto = new Response();
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
                // Lấy toàn bộ các Subject liên kết với Tool này
                var toolSubjects = await _toolForSubjectRepository.GetAllDataByExpression(
                    filter: x => x.ToolId == toolId,
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);
                if (toolSubjects == null || !toolSubjects.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No subjects found for the specified tool.";
                    return dto;
                }
                // Xoá toàn bộ
                await _toolForSubjectRepository.DeleteRange(toolSubjects.Items);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Message = "All subjects have been deleted from the tool.";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting subjects from the tool: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteAllToolsFromSubject(Guid subjectId)
        {
            var dto = new Response();
            try
            {
                var subject = await _subjectRepository.GetById(subjectId);
                if (subject == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Subject not found";
                    return dto;
                }

                var toolSubjects = await _toolForSubjectRepository.GetAllDataByExpression(
                    filter: x => x.SubjectId == subjectId,
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);

                if (toolSubjects == null || !toolSubjects.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No tools found for the specified subject.";
                    return dto;
                }

                await _toolForSubjectRepository.DeleteRange(toolSubjects.Items);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Message = "All tools have been deleted from the subject.";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting tools from the subject: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteSubjectsFromTool(Guid toolId, List<Guid> subjectIds)
        {
            var dto = new Response();
            try
            {
                var tool = await _toolRepository.GetById(toolId);
                if (tool == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Tool not found.";
                    return dto;
                }

                // Lấy danh sách liên kết Tool - Subject theo danh sách subjectIds
                var toolSubjects = await _toolForSubjectRepository.GetAllDataByExpression(
                    filter: x => x.ToolId == toolId && subjectIds.Contains((Guid)x.SubjectId),
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);

                if (toolSubjects == null || !toolSubjects.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No matching subject links found for the tool.";
                    return dto;
                }

                await _toolForSubjectRepository.DeleteRange(toolSubjects.Items);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = toolSubjects.Items.Count;
                dto.Message = "Subjects have been deleted from the tool.";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting subjects from the tool: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> DeleteToolsFromSubject(Guid subjectId, List<Guid> toolIds)
        {
            var dto = new Response();
            try
            {
                var subject = await _subjectRepository.GetById(subjectId);
                if (subject == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Subject not found.";
                    return dto;
                }

                var toolForSubjects = await _toolForSubjectRepository.GetAllDataByExpression(
                    filter: x => x.SubjectId == subjectId && toolIds.Contains((Guid)x.ToolId),
                    pageNumber: 1,
                    pageSize: int.MaxValue,
                    orderBy: null,
                    isAscending: true,
                    includes: null);

                if (toolForSubjects == null || !toolForSubjects.Items.Any())
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "No matching tool links found for the subject.";
                    return dto;
                }

                await _toolForSubjectRepository.DeleteRange(toolForSubjects.Items);
                await _unitOfWork.SaveChangeAsync();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.DELETE_SUCCESSFULLY;
                dto.Data = toolForSubjects.Items.Count;
                dto.Message = "Tools have been deleted from the subject.";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while deleting tools from the subject: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllSubjectsForTool(Guid toolId, int pageNumber, int pageSize)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _toolForSubjectRepository.GetAllDataByExpression(
                    filter: t => t.ToolId == toolId,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: null,
                    isAscending: true,
                    includes: new Expression<Func<ToolForSubject, object>>[]
                {
                    c => c.Tool,
                    c => c.Subject
                });
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Subjects For Tool retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving Subjects For Tool: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetAllToolsForSubject(Guid subjectId, int pageNumber, int pageSize)
        {
            Response dto = new Response();
            try
            {
                dto.Data = await _toolForSubjectRepository.GetAllDataByExpression(
                    filter: x => x.SubjectId == subjectId,
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    orderBy: null,
                    isAscending: true,
                    includes: new Expression<Func<ToolForSubject, object>>[]
                {
                    c => c.Tool,
                    c => c.Subject
                });
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Tools for Subject retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving Tools for Subject: " + ex.Message;
            }
            return dto;
        }

        public async Task<Response> GetToolForSubjectById(Guid toolForSubjectId)
        {
            Response dto = new Response();
            try
            {
                var toolForSubject = await _toolForSubjectRepository.GetById(toolForSubjectId);
                if (toolForSubject == null)
                {
                    dto.IsSucess = false;
                    dto.BusinessCode = BusinessCode.DATA_NOT_FOUND;
                    dto.Message = "Tool For Subject not found";
                    return dto;
                }
                dto.Data = toolForSubject;
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
                dto.Message = "Tool For Subject retrieved successfully";
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Message = "An error occurred while retrieving Tool For Subject: " + ex.Message;
            }
            return dto;
        }
    }
}
