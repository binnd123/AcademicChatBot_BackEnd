using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Subjects;
using AcademicChatBot.Service.Contract;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/subject")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }


        [Authorize]
        [HttpGet("subjects")]
        public async Task<IActionResult> GetAllSubjects(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = ""
            )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            var response = await _subjectService.GetAllSubjects(pageNumber, pageSize, search);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("subject/{id}")]
        public async Task<IActionResult> GetSubjectById(Guid id)
        {
            var response = await _subjectService.GetSubjectById(id);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPost("subject")]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectRequest request)
        {
            var response = await _subjectService.CreateSubject(request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPut("subject/{id}")]
        public async Task<IActionResult> UpdateSubject(Guid id, UpdateSubjectRequest request)
        {
            var response = await _subjectService.UpdateSubject(id, request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpDelete("subject/{id}")]
        public async Task<IActionResult> DeleteSubject(Guid id)
        {
            var response = await _subjectService.DeleteSubject(id);
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
