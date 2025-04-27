using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.PrerequisiteConstraint;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/prerequisite-constraints")]
    [ApiController]
    public class PrerequisiteConstraintController : ControllerBase
    {
        private readonly IPrerequisiteConstraintService _prerequisiteConstraintService;

        public PrerequisiteConstraintController(IPrerequisiteConstraintService prerequisiteConstraintService)
        {
            _prerequisiteConstraintService = prerequisiteConstraintService;
        }

        // Lấy tất cả prerequisite constraints với phân trang và tìm kiếm
        [HttpGet]
        public async Task<IActionResult> GetAllPrerequisiteConstraints(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = "",
            [FromQuery] SortBy sortBy = SortBy.Default,
            [FromQuery] SortType sortType = SortType.Ascending,
            [FromQuery] bool isDelete = false)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            var response = await _prerequisiteConstraintService.GetAllPrerequisiteConstraints(pageNumber, pageSize, search, sortBy, sortType, isDelete);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Lấy thông tin prerequisite constraint theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrerequisiteConstraintById(Guid id)
        {
            var response = await _prerequisiteConstraintService.GetPrerequisiteConstraintById(id);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Tạo mới prerequisite constraint
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreatePrerequisiteConstraint([FromBody] CreatePrerequisiteConstraintRequest request)
        {
            var response = await _prerequisiteConstraintService.CreatePrerequisiteConstraint(request);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Cập nhật prerequisite constraint
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrerequisiteConstraint(Guid id, [FromBody] UpdatePrerequisiteConstraintRequest request)
        {
            var response = await _prerequisiteConstraintService.UpdatePrerequisiteConstraint(id, request);
            if (!response.IsSucess)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        // Xóa prerequisite constraint
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrerequisiteConstraint(Guid id)
        {
            var response = await _prerequisiteConstraintService.DeletePrerequisiteConstraint(id);
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
