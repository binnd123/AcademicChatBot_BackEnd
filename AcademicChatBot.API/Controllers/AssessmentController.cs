using AcademicChatBot.Common.BussinessModel.Assessment;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/assessment")]
    [ApiController]
    public class AssessmentController : ControllerBase
    {
        private readonly IAssessmentService _assessmentService;

        public AssessmentController(IAssessmentService assessmentService)
        {
            _assessmentService = assessmentService;
        }

        [Authorize(Roles = "Admin,Student")]
        [HttpGet("get-assessment/{id}")]
        public async Task<Response> GetAssessmentById(Guid id)
        {
            return await _assessmentService.GetAssessmentById(id);
        }

        [HttpGet("get-all-assessments")]
        public async Task<Response> GetAllAssessments(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = ""
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            return await _assessmentService.GetAllAssessments(pageNumber, pageSize, search);
        }

        [HttpPost("create-assessment")]
        public async Task<Response> CreateAssessment([FromBody] CreateAssessmentRequest request)
        {
            return await _assessmentService.CreateAssessment(request);
        }

        [HttpPut("update-assessment/{id}")]
        public async Task<Response> UpdateAssessment(Guid id, [FromBody] UpdateAssessmentRequest request)
        {
            return await _assessmentService.UpdateAssessment(id, request);
        }

        [HttpDelete("delete-assessment/{id}")]
        public async Task<Response> DeleteAssessment(Guid id)
        {
            return await _assessmentService.DeleteAssessment(id);
        }
    }
}
