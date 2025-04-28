using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.ComboSubject;

namespace AcademicChatBot.Service.Contract
{
    public interface IComboSubjectService
    {
        public Task<Response> GetAllCombosForSubject(Guid subjectId, int pageNumber, int pageSize);
        public Task<Response> GetAllSubjectsForCombo(Guid comboId, int pageNumber, int pageSize);
        public Task<Response> GetComboSubjectById(Guid ComboSubjectId);
        public Task<Response> AddComboSubject(Guid comboId, Guid subjectId, int semesterNo, string note);
        public Task<Response> DeleteCombosFromSubject(Guid subjectId, List<Guid> comboIds);
        public Task<Response> DeleteSubjectsFromCombo(Guid comboId, List<Guid> subjectIds);
        public Task<Response> DeleteAllCombosFromSubject(Guid subjectId);
        public Task<Response> DeleteAllSubjectsFromCombo(Guid comboId);
        public Task<Response> AddSubjectsToCombo(Guid comboId, List<SubjectInComboRequest> requests);
    }
}
