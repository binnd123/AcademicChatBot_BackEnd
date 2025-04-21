using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Curriculum;

namespace AcademicChatBot.Service.Contract
{
    public interface ICurriculumService
    {
        Task<Response> CreateCurriculum(CreateCurriculumRequest request);
        Task<Response> DeleteCurriculum(Guid curriculumId);
        Task<Response> GetAllCurriculums(int pageNumber, int pageSize, string search);
        Task<Response> GetCurriculumById(Guid curriculumId);
        Task<Response> UpdateCurriculum(Guid curriculumId, UpdateCurriculumRequest request);
        Task<Response> GetCurriculumByCode(int pageNumber, int pageSize, string curriculumCode);
    }
}
