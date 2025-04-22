using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel.ProgramingLearningOutcome;
using AcademicChatBot.Common.DTOs;

namespace AcademicChatBot.Service.Contract
{
    public interface IProgramingLearningOutcomeService
    {
        Task<Response> CreateProgramingLearningOutcome(CreateProgramingLearningOutcomeRequest request);
        Task<Response> DeleteProgramingLearningOutcome(Guid programingLearningOutcomeId);
        Task<Response> GetAllProgramingLearningOutcomes(int pageNumber, int pageSize, string search);
        Task<Response> GetProgramingLearningOutcomeById(Guid programingLearningOutcomeId);
        Task<Response> UpdateProgramingLearningOutcome(Guid programingLearningOutcomeId, UpdateProgramingLearningOutcomeRequest request);
    }
}
