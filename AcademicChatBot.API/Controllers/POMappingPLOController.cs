using AcademicChatBot.Common.BussinessCode;
using AcademicChatBot.DAL.Models;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Route("api/po-mapping-plo")]
    [ApiController]
    public class POMappingPLOController : ControllerBase
    {
        private readonly IPOMappingPLOService _poMappingPLOService;

        public POMappingPLOController(IPOMappingPLOService poMappingPLOService)
        {
            _poMappingPLOService = poMappingPLOService;
        }

        [HttpGet("get-po-mapping-plo-by-id/{pOMappingPLOId}")]
        public async Task<IActionResult> GetPOMappingPLOById(Guid pOMappingPLOId)
        {
            var response = await _poMappingPLOService.GetPOMappingPLOById(pOMappingPLOId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-all-plo-for-po")]
        public async Task<IActionResult> GetAllPLOsForPO(
            [FromQuery] Guid pOId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            var response = await _poMappingPLOService.GetAllPLOsForPO(pOId, pageNumber, pageSize);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-all-po-for-plo")]
        public async Task<IActionResult> GetAllPOsForPLO(
            [FromQuery] Guid pLOId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            var response = await _poMappingPLOService.GetAllPOsForPLO(pLOId, pageNumber, pageSize);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add-plo-to-po")]
        public async Task<IActionResult> AddPLOsToPO(
            [FromQuery] Guid pOId,
            [FromBody] List<Guid> pLOIds)
        {
            var response = await _poMappingPLOService.AddPLOsToPO(pOId, pLOIds);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add-po-to-plo")]
        public async Task<IActionResult> AddPOsToPLO(
        [FromQuery] Guid pLOId,
            [FromBody] List<Guid> pOIds)
        {
            var response = await _poMappingPLOService.AddPOsToPLO(pLOId, pOIds);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-plo-from-po")]
        public async Task<IActionResult> DeletePLOsFromPO(
            [FromQuery] Guid pOId,
            [FromBody] List<Guid> pLOIds)
        {
            var response = await _poMappingPLOService.DeletePLOsFromPO(pOId, pLOIds);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-po-from-plo")]
        public async Task<IActionResult> DeletePOsFromPLO(
        [FromQuery] Guid pLOId,
            [FromBody] List<Guid> pOIds)
        {
            var response = await _poMappingPLOService.DeletePOsFromPLO(pLOId, pOIds);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-all-plo-from-po")]
        public async Task<IActionResult> DeleteAllPLOsFromPO(
            [FromQuery] Guid pOId)
        {
            var response = await _poMappingPLOService.DeleteAllPLOsFromPO(pOId);
            if (response.IsSucess == false)
            {
                if (response.BusinessCode == BusinessCode.EXCEPTION)
                    return StatusCode(500, response);
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-all-po-from-plo")]
        public async Task<IActionResult> DeleteAllPOsFromPLO(
            [FromQuery] Guid pLOId)
        {
            var response = await _poMappingPLOService.DeleteAllPOsFromPLO(pLOId);
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
