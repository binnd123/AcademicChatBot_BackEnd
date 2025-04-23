using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.PrerequisiteConstraint;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Contract
{
    public interface IPrerequisiteConstraintService
    {
        Task<Response> CreatePrerequisiteConstraint(CreatePrerequisiteConstraintRequest request);
        Task<Response> GetAllPrerequisiteConstraints(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDelete);
        Task<Response> GetPrerequisiteConstraintById(Guid id);
        Task<Response> UpdatePrerequisiteConstraint(Guid id, UpdatePrerequisiteConstraintRequest request);
        Task<Response> DeletePrerequisiteConstraint(Guid id);
    }
}
