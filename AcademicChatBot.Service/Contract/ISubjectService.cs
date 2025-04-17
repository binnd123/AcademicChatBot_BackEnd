using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.DTOs;
using AcademicChatBot.Common.DTOs.Subjects;

namespace AcademicChatBot.Service.Contract
{
    public interface ISubjectService
    {
        Task<ResponseDTO> GetAllSubjects(int pageNumber, int pageSize, string search);
        Task<ResponseDTO> GetSubjectById(Guid subjectId);
        public Task<ResponseDTO> CreateSubject(CreateSubjectRequest request);
        public Task<ResponseDTO> UpdateSubject(Guid SubjectId, UpdateSubjectRequest request);
        public Task<ResponseDTO> DeleteSubject(Guid SubjectId);
    }
}
