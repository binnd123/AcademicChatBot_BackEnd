using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.PrerequisiteSubject;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Contract
{
    public interface IPrerequisiteSubjectService
    {
        Task<Response> CreatePrerequisiteSubject(CreatePrerequisiteSubjectRequest request);
        Task<Response> GetAllPrerequisiteSubjects(int pageNumber, int pageSize, SortBy sortBy, SortType sortType, bool isDelete);
        Task<Response> GetPrerequisiteSubjectById(Guid id);
        Task<Response> UpdatePrerequisiteSubject(Guid id, UpdatePrerequisiteSubjectRequest request);
        Task<Response> DeletePrerequisiteSubject(Guid id);
    }
}
