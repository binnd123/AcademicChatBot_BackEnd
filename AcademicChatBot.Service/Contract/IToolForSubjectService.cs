using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Contract
{
    public interface IToolForSubjectService
    {
        public Task<Response> GetAllToolsForSubject(Guid subjectId, int pageNumber, int pageSize);
        public Task<Response> GetAllSubjectsForTool(Guid toolId, int pageNumber, int pageSize);
        public Task<Response> GetToolForSubjectById(Guid toolForSubjectId);
        public Task<Response> AddSubjectsToTool(Guid toolId, List<Guid> subjectIds);
        public Task<Response> AddToolsToSubject(Guid subjectId, List<Guid> toolIds);
        public Task<Response> DeleteToolsFromSubject(Guid subjectId, List<Guid> toolIds);
        public Task<Response> DeleteSubjectsFromTool(Guid toolId, List<Guid> subjectIds);
        public Task<Response> DeleteAllToolsFromSubject(Guid subjectId);
        public Task<Response> DeleteAllSubjectsFromTool(Guid toolId);

    }
}
