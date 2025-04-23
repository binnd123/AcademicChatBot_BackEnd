using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Combo;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Contract
{
    public interface IComboService
    {
        Task<Response> CreateCombo(CreateComboRequest request);
        Task<Response> GetAllCombos(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDelete);
        Task<Response> GetComboById(Guid id);
        Task<Response> UpdateCombo(Guid id, UpdateComboRequest request);
        Task<Response> DeleteCombo(Guid id);
    }
}
