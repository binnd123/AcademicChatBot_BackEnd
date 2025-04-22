using AcademicChatBot.Common.BussinessModel.ProgramingOutcome;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/programing-outcome")]
    [ApiController]
    public class ProgramingOutcomeController : ControllerBase
    {
        private readonly IProgramingOutcomeService _programingOutcomeService;

        public ProgramingOutcomeController(IProgramingOutcomeService programingOutcomeService)
        {
            _programingOutcomeService = programingOutcomeService;
        }

        [Authorize(Roles = "Admin,Student")]
        [HttpGet("get-programing-outcome/{id}")]
        public async Task<Response> GetProgramingOutcomeById(Guid id)
        {
            return await _programingOutcomeService.GetProgramingOutcomeById(id);
        }

        [HttpGet("get-all-programing-outcomes")]
        public async Task<Response> GetAllProgramingOutcomes(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = ""
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            return await _programingOutcomeService.GetAllProgramingOutcomes(pageNumber, pageSize, search);
        }

        [HttpPost("create-programing-outcome")]
        public async Task<Response> CreateProgramingOutcome([FromBody] CreateProgramingOutcomeRequest request)
        {
            return await _programingOutcomeService.CreateProgramingOutcome(request);
        }

        [HttpPut("update-programing-outcome/{id}")]
        public async Task<Response> UpdateProgramingOutcome(Guid id, [FromBody] UpdateProgramingOutcomeRequest request)
        {
            return await _programingOutcomeService.UpdateProgramingOutcome(id, request);
        }

        [HttpDelete("delete-programing-outcome/{id}")]
        public async Task<Response> DeleteProgramingOutcome(Guid id)
        {
            return await _programingOutcomeService.DeleteProgramingOutcome(id);
        }
    }
}
