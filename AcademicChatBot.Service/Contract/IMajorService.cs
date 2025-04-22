using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel.Major;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Contract
{
    public interface IMajorService
    {
        Task<Response> CreateMajor(CreateMajorRequest request);
        Task<Response> DeleteMajor(Guid majorId);
        Task<Response> GetAllMajors(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType);
        Task<Response> GetMajorById(Guid majorId);
        Task<Response> UpdateMajor(Guid majorId, UpdateMajorRequest request);
    }
}
