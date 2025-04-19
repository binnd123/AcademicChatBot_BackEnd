using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.Curriculum;

namespace AcademicChatBot.Service.Contract
{
    public interface ICurriculumService
    {
        Task<ResponseDTO> CreateCurriculum(CreateCurriculumRequest request);
        Task<ResponseDTO> DeleteCurriculum(Guid curriculumId);
        Task<ResponseDTO> GetAllCurriculum(int pageNumber, int pageSize, string search);
        Task<ResponseDTO> GetCurriculumById(Guid curriculumId);
        Task<ResponseDTO> UpdateCurriculum(Guid curriculumId, UpdateCurriculumRequest request);
    }
}
