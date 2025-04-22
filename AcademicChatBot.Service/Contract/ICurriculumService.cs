using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Curriculum;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Contract
{
    public interface ICurriculumService
    {
        Task<Response> CreateCurriculum(CreateCurriculumRequest request);
        Task<Response> DeleteCurriculum(Guid curriculumId);
        Task<Response> GetAllCurriculums(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType);
        Task<Response> GetCurriculumById(Guid curriculumId);
        Task<Response> UpdateCurriculum(Guid curriculumId, UpdateCurriculumRequest request);
        Task<Response> GetCurriculumByCode(int pageNumber, int pageSize, string curriculumCode, SortBy sortBy, SortType sortType);
    }
}
