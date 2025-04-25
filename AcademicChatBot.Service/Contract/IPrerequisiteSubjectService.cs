using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.PrerequisiteSubject;
using AcademicChatBot.Common.BussinessModel.SubjectInCurriculum;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Contract
{
    public interface IPrerequisiteSubjectService
    {
        //Task<Response> CreatePrerequisiteSubject(CreatePrerequisiteSubjectRequest request);
        //Task<Response> GetAllPrerequisiteSubjects(int pageNumber, int pageSize, SortBy sortBy, SortType sortType, bool isDelete);
        //Task<Response> UpdatePrerequisiteSubject(Guid id, UpdatePrerequisiteSubjectRequest request);
        //Task<Response> DeletePrerequisiteSubject(Guid id);
        Task<Response> GetPrerequisiteSubjectById(Guid id);
        Task<Response> GetReadablePrerequisiteExpression(Guid prerequisiteConstrainId);
        Task<Response> GetReadablePrerequisiteExpressionOfSubjectInCurriculum(Guid subjectId, Guid curriculumId);
        Task<Response> GetAllPrerequisiteSubjectsForPrerequisiteConstrain(Guid prerequisiteConstrainId, int pageNumber, int pageSize);
        Task<Response> AddPrerequisiteSubjectsToPrerequisiteConstrain(Guid prerequisiteConstrainId, List<PrerequisiteSubjectsToPrerequisiteConstrainRequest> requests);
        Task<Response> DeletePrerequisiteSubjectsFromPrerequisiteConstrain(Guid prerequisiteConstrainId, List<Guid> prerequisiteSubjectIds);
        Task<Response> DeleteAllPrerequisiteSubjectsFromPrerequisiteConstrain(Guid prerequisiteConstrainId);
    }
}
