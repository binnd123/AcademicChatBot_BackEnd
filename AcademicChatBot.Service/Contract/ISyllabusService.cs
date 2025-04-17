using System;
using System.Threading.Tasks;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.Syllabus;

namespace AcademicChatBot.Service.Contract
{
    public interface ISyllabusService
    {
        Task<ResponseDTO> CreateSyllabus(CreateSyllabusRequest request);
        Task<ResponseDTO> DeleteSyllabus(Guid syllabusId);
        Task<ResponseDTO> GetAllSyllabi(int pageNumber, int pageSize, string search);
        Task<ResponseDTO> GetSyllabusById(Guid syllabusId);
        Task<ResponseDTO> UpdateSyllabus(Guid syllabusId, UpdateSyllabusRequest request);
    }
}
