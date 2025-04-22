using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Curriculum;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/curriculum")]
    [ApiController]
    public class CurriculumController : ControllerBase
    {
        private readonly ICurriculumService _curriculumService;

        public CurriculumController(ICurriculumService curriculumService)
        {
            _curriculumService = curriculumService;
        }

        [Authorize]
        [HttpGet("get-curriculum/{curriculumId}")]
        public async Task<IActionResult> GetCurriculumById(Guid curriculumId)
        {
            var response = await _curriculumService.GetCurriculumById(curriculumId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-all-curriculums")]
        public async Task<IActionResult> GetAllCurriculums(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = "",
            [FromQuery] SortBy sortBy = SortBy.Default,
            [FromQuery] SortType sortType = SortType.Ascending
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            var response = await _curriculumService.GetAllCurriculums(pageNumber, pageSize, search, sortBy, sortType);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        //[HttpGet("get-curriculum-by-code")]
        //public async Task<Response> GetCurriculumByCode(
        //    [FromQuery] int pageNumber = 1,
        //    [FromQuery] int pageSize = 5,
        //    [FromQuery] string curriculumCode = ""
        //)
        //{
        //    pageNumber = pageNumber < 1 ? 1 : pageNumber;
        //    pageSize = pageSize < 1 ? 5 : pageSize;
        //    return await _curriculumService.GetCurriculumByCode(pageNumber, pageSize, curriculumCode);
        //}

        [Authorize(Roles = "Admin")]
        [HttpPost("create-curriculum")]
        public async Task<IActionResult> CreateCurriculum([FromBody] CreateCurriculumRequest request)
        {
            var response = await _curriculumService.CreateCurriculum(request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("update-curriculum/{curriculumId}")]
        public async Task<IActionResult> UpdateCurriculum(Guid curriculumId, [FromBody] UpdateCurriculumRequest request)
        {
            var response = await _curriculumService.UpdateCurriculum(curriculumId, request);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-curriculum/{curriculumId}")]
        public async Task<IActionResult> DeleteCurriculum(Guid curriculumId)
        {
            var response = await _curriculumService.DeleteCurriculum(curriculumId);
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
