using AcademicChatBot.Common.BussinessModel.Tools;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.Tool;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/tool")]
    [ApiController]
    public class ToolController : ControllerBase
    {
        private readonly IToolService _toolService;

        public ToolController(IToolService toolService)
        {
            _toolService = toolService;
        }

        [Authorize(Roles = "Admin,Student")]
        [HttpGet("get-tool/{id}")]
        public async Task<Response> GetToolById(Guid id)
        {
            return await _toolService.GetToolById(id);
        }

        [HttpGet("get-all-tools")]
        public async Task<Response> GetAllTools(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = ""
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            return await _toolService.GetAllTools(pageNumber, pageSize, search);
        }

        [HttpPost("create-tool")]
        public async Task<Response> CreateTool([FromBody] CreateToolRequest request)
        {
            return await _toolService.CreateTool(request);
        }

        [HttpPut("update-tool/{id}")]
        public async Task<Response> UpdateTool(Guid id, [FromBody] UpdateToolRequest request)
        {
            return await _toolService.UpdateTool(id, request);
        }

        [HttpPut("delete-tool/{id}")]
        public async Task<Response> DeleteTool(Guid id)
        {
            return await _toolService.DeleteTool(id);
        }
    }
}
