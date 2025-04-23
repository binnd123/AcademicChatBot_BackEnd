using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Tools;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Contract
{
    public interface IToolService
    {
        Task<Response> CreateTool(CreateToolRequest request);
        Task<Response> DeleteTool(Guid toolId);
        Task<Response> GetAllTools(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDelete);
        Task<Response> GetToolById(Guid toolId);
        Task<Response> UpdateTool(Guid toolId, UpdateToolRequest request);
    }
}
