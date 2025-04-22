using AcademicChatBot.Common.BussinessModel.Major;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/major")]
    [ApiController]
    public class MajorController : ControllerBase
    {
        private readonly IMajorService _majorService;

        public MajorController(IMajorService majorService)
        {
            _majorService = majorService;
        }

        [Authorize(Roles = "Admin,Student")]
        [HttpGet("get-major/{id}")]
        public async Task<Response> GetMajorById(Guid id)
        {
            return await _majorService.GetMajorById(id);
        }

        [HttpGet("get-all-majors")]
        public async Task<Response> GetAllMajors(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = ""
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            return await _majorService.GetAllMajors(pageNumber, pageSize, search);
        }

        [HttpPost("create-major")]
        public async Task<Response> CreateMajor([FromBody] CreateMajorRequest request)
        {
            return await _majorService.CreateMajor(request);
        }

        [HttpPut("update-major/{id}")]
        public async Task<Response> UpdateMajor(Guid id, [FromBody] UpdateMajorRequest request)
        {
            return await _majorService.UpdateMajor(id, request);
        }

        [HttpDelete("delete-major/{id}")]
        public async Task<Response> DeleteMajor(Guid id)
        {
            return await _majorService.DeleteMajor(id);
        }
    }
}
