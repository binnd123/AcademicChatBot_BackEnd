using AcademicChatBot.Common.BussinessModel.Material;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/material")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [Authorize(Roles = "Admin,Student")]
        [HttpGet("get-material/{id}")]
        public async Task<Response> GetMaterialById(Guid id)
        {
            return await _materialService.GetMaterialById(id);
        }

        [HttpGet("get-all-materials")]
        public async Task<Response> GetAllMaterials(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = ""
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            return await _materialService.GetAllMaterials(pageNumber, pageSize, search);
        }

        [HttpPost("create-material")]
        public async Task<Response> CreateMaterial([FromBody] CreateMaterialRequest request)
        {
            return await _materialService.CreateMaterial(request);
        }

        [HttpPut("update-material/{id}")]
        public async Task<Response> UpdateMaterial(Guid id, [FromBody] UpdateMaterialRequest request)
        {
            return await _materialService.UpdateMaterial(id, request);
        }

        [HttpDelete("delete-material/{id}")]
        public async Task<Response> DeleteMaterial(Guid id)
        {
            return await _materialService.DeleteMaterial(id);
        }
    }
}
