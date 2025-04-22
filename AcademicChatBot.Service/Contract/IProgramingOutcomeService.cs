using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel.ProgramingOutcome;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Contract
{
    public interface IProgramingOutcomeService
    {
        Task<Response> CreateProgramingOutcome(CreateProgramingOutcomeRequest request);
        Task<Response> DeleteProgramingOutcome(Guid programingOutcomeId);
        Task<Response> GetAllProgramingOutcomes(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType);
        Task<Response> GetProgramingOutcomeById(Guid programingOutcomeId);
        Task<Response> UpdateProgramingOutcome(Guid programingOutcomeId, UpdateProgramingOutcomeRequest request);
    }
}
