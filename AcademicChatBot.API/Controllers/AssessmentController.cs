using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Assessment;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/assessment")]
    [ApiController]
    public class AssessmentController : ControllerBase
    {
        private readonly IAssessmentService _assessmentService;

        public AssessmentController(IAssessmentService assessmentService)
        {
            _assessmentService = assessmentService;
        }

        [Authorize]
        [HttpGet("get-assessment-by-id/{assessmentId}")]
        public async Task<IActionResult> GetAssessmentById(Guid assessmentId)
        {
            var response = await _assessmentService.GetAssessmentById(assessmentId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("get-all-assessments")]
        public async Task<IActionResult> GetAllAssessments(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = "",
            [FromQuery] bool isDelete = false
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            var response = await _assessmentService.GetAllAssessments(pageNumber, pageSize, search, isDelete);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-assessment")]
        public async Task<IActionResult> CreateAssessment([FromBody] CreateAssessmentRequest request)
        {
            var response = await _assessmentService.CreateAssessment(request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-assessment/{assessmentId}")]
        public async Task<IActionResult> UpdateAssessment(Guid assessmentId, UpdateAssessmentRequest request)
        {
            var response = await _assessmentService.UpdateAssessment(assessmentId, request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-assessment/{assessmentId}")]
        public async Task<IActionResult> DeleteAssessment(Guid assessmentId)
        {
            var response = await _assessmentService.DeleteAssessment(assessmentId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
