using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.SubjectInCurriculum;

namespace AcademicChatBot.Service.Contract
{
    public interface ISubjectInCurriculumService
    {
        Task<Response> GetSubjectInCurriculumById(Guid subjectInCurriculumId);
        Task<Response> GetAllSubjectsForCurriculum(Guid curriculumId, int pageNumber, int pageSize, int semesterNo);
        Task<Response> GetAllCurriculumsForSubject(Guid subjectId, int pageNumber, int pageSize, int semesterNo);
        Task<Response> AddSubjectsToCurriculum(Guid curriculumId, List<SubjectInCurriculumRequest> requests);
        Task<Response> AddCurriculumsToSubject(Guid subjectId, List<CurriculumInSubjectRequest> requests);
        Task<Response> DeleteSubjectsFromCurriculum(Guid curriculumId, List<Guid> subjectIds);
        Task<Response> DeleteCurriculumsFromSubject(Guid subjectId, List<Guid> curriculumIds);
        Task<Response> DeleteAllSubjectsFromCurriculum(Guid curriculumId);
        Task<Response> DeleteAllCurriculumsFromSubject(Guid subjectId);
    }
}
