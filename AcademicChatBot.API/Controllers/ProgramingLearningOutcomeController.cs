using AcademicChatBot.Common.BussinessModel.ProgramingLearningOutcome;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicChatBot.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/programing-learning-outcome")]
    [ApiController]
    public class ProgramingLearningOutcomeController : ControllerBase
    {
        private readonly IProgramingLearningOutcomeService _programingLearningOutcomeService;

        public ProgramingLearningOutcomeController(IProgramingLearningOutcomeService programingLearningOutcomeService)
        {
            _programingLearningOutcomeService = programingLearningOutcomeService;
        }

        [Authorize(Roles = "Admin,Student")]
        [HttpGet("get-programing-learning-outcome/{id}")]
        public async Task<Response> GetProgramingLearningOutcomeById(Guid id)
        {
            return await _programingLearningOutcomeService.GetProgramingLearningOutcomeById(id);
        }

        [HttpGet("get-all-programing-learning-outcomes")]
        public async Task<Response> GetAllProgramingLearningOutcomes(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 5,
            [FromQuery] string search = ""
        )
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;
            return await _programingLearningOutcomeService.GetAllProgramingLearningOutcomes(pageNumber, pageSize, search);
        }

        [HttpPost("create-programing-learning-outcome")]
        public async Task<Response> CreateProgramingLearningOutcome([FromBody] CreateProgramingLearningOutcomeRequest request)
        {
            return await _programingLearningOutcomeService.CreateProgramingLearningOutcome(request);
        }

        [HttpPut("update-programing-learning-outcome/{id}")]
        public async Task<Response> UpdateProgramingLearningOutcome(Guid id, [FromBody] UpdateProgramingLearningOutcomeRequest request)
        {
            return await _programingLearningOutcomeService.UpdateProgramingLearningOutcome(id, request);
        }

        [HttpDelete("delete-programing-learning-outcome/{id}")]
        public async Task<Response> DeleteProgramingLearningOutcome(Guid id)
        {
            return await _programingLearningOutcomeService.DeleteProgramingLearningOutcome(id);
        }
    }
}
