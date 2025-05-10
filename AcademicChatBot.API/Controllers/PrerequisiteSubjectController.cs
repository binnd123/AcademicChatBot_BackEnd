using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel.PrerequisiteSubject;
using AcademicChatBot.Common.BussinessModel.SubjectInCurriculum;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/prerequisite-subjects")]
    [ApiController]
    public class PrerequisiteSubjectController : ControllerBase
    {
        private readonly IPrerequisiteSubjectService _prerequisiteSubjectService;

        public PrerequisiteSubjectController(IPrerequisiteSubjectService prerequisiteSubjectService)
        {
            _prerequisiteSubjectService = prerequisiteSubjectService;
        }

        // Lấy thông tin prerequisite expression cho constraint
        [HttpGet("expressions-for-constrain")]
        public async Task<IActionResult> GetReadablePrerequisiteExpression(
            [FromQuery] Guid prerequisiteConstrainId)
        {
            var response = await _prerequisiteSubjectService.GetReadablePrerequisiteExpression(prerequisiteConstrainId);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Lấy thông tin prerequisite expression của môn học trong chương trình học
        [HttpGet("expressions-of-subject")]
        public async Task<IActionResult> GetReadablePrerequisiteExpressionOfSubject(
            [FromQuery] Guid subjectId)
        {
            var response = await _prerequisiteSubjectService.GetReadablePrerequisiteExpressionOfSubject(subjectId);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Thêm môn học vào constraint prerequisite
        [Authorize(Roles = "Admin")]
        [HttpPost("subjects-to-constrain")]
        public async Task<IActionResult> AddPrerequisiteSubjectsToPrerequisiteConstrain(
            [FromQuery] Guid prerequisiteConstrainId,
            [FromBody] List<PrerequisiteSubjectsToPrerequisiteConstrainRequest> requests)
        {
            var response = await _prerequisiteSubjectService.AddPrerequisiteSubjectsToPrerequisiteConstrain(prerequisiteConstrainId, requests);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return BadRequest(response);
            }
            return Ok(response);
        }

        // Xóa tất cả môn học khỏi prerequisite constraint
        [Authorize(Roles = "Admin")]
        [HttpDelete("subjects-from-constrain")]
        public async Task<IActionResult> DeleteAllPrerequisiteSubjectsFromPrerequisiteConstrain([FromQuery] Guid prerequisiteConstrainId)
        {
            var response = await _prerequisiteSubjectService.DeleteAllPrerequisiteSubjectsFromPrerequisiteConstrain(prerequisiteConstrainId);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }
    }

}
