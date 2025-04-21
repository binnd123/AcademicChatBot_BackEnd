using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Subjects;

namespace AcademicChatBot.Service.Contract
{
    public interface IProgramService
    {
        Task<Response> GetAllPrograms(int pageNumber, int pageSize, string search);
        Task<Response> GetProgramById(Guid subjectId);
        public Task<Response> CreateProgram(CreateSubjectRequest request);
        public Task<Response> UpdateProgram(Guid SubjectId, UpdateSubjectRequest request);
        public Task<Response> DeleteProgram(Guid SubjectId);
    }
}
