using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Subjects;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Contract
{
    public interface ISubjectService
    {
        Task<Response> GetAllSubjects(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType);
        Task<Response> GetSubjectById(Guid subjectId);
        public Task<Response> CreateSubject(CreateSubjectRequest request);
        public Task<Response> UpdateSubject(Guid SubjectId, UpdateSubjectRequest request);
        public Task<Response> DeleteSubject(Guid SubjectId);
    }
}
