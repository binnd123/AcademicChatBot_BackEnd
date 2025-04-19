using AcademicChatBot.Common.BussinessModel.Curriculum;
using AcademicChatBot.Common.DTOs;

namespace AcademicChatBot.Service.Contract
{
    public interface ICurriculumService
    {
        Task<Response> CreateCurriculum(CreateCurriculumRequest request);
        Task<Response> DeleteCurriculum(Guid curriculumId);
        Task<Response> GetAllCurriculums(int pageNumber, int pageSize, string search);
        Task<Response> GetCurriculumById(Guid curriculumId);
        Task<Response> UpdateCurriculum(Guid curriculumId, UpdateCurriculumRequest request);
    }
}
