﻿using AcademicChatBot.Common.BussinessModel.Curriculum;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/curriculum")]
    [ApiController]
    public class CurriculumController : ControllerBase
    {
        private readonly ICurriculumService _curriculumService;

        public CurriculumController(ICurriculumService curriculumService)
        {
            _curriculumService = curriculumService;
        }

        [Authorize(Roles = "Admin,Student")]
        [HttpGet("get-curriculum/{id}")]
        public async Task<Response> GetCurriculumById(Guid id)
        {
            return await _curriculumService.GetCurriculumById(id);
        }

        [HttpGet("get-all-curriculum")]
        public async Task<Response> GetAllCurriculums(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = ""
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            return await _curriculumService.GetAllCurriculums(pageNumber, pageSize, search);
        }

        [HttpPost("create-curriculum")]
        public async Task<Response> CreateCurriculum([FromBody] CreateCurriculumRequest request)
        {
            return await _curriculumService.CreateCurriculum(request);
        }

        [HttpPut("update-curriculum/{id}")]
        public async Task<Response> UpdateCurriculum(Guid id, [FromBody] UpdateCurriculumRequest request)
        {
            return await _curriculumService.UpdateCurriculum(id, request);
        }

        [HttpPut("delete-curriculum/{id}")]
        public async Task<Response> DeleteCurriculum(Guid id)
        {
            return await _curriculumService.DeleteCurriculum(id);
        }
    }
}
