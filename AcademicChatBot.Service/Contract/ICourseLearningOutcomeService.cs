using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.CourseLearningOutcome;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Contract
{
    public interface ICourseLearningOutcomeService
    {
        Task<Response> CreateCourseLearningOutcome(CreateCourseLearningOutcomeRequest request);
        Task<Response> DeleteCourseLearningOutcome(Guid cloId);
        Task<Response> GetAllCourseLearningOutcomes(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDeleted);
        Task<Response> GetCourseLearningOutcomeById(Guid cloId);
        Task<Response> UpdateCourseLearningOutcome(Guid cloId, UpdateCourseLearningOutcomeRequest request);
    }
}
