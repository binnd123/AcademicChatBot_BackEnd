using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Programs;
using AcademicChatBot.Common.BussinessModel.Subjects;
using AcademicChatBot.Common.Enum;

namespace AcademicChatBot.Service.Contract
{
    public interface IProgramService
    {
        Task<Response> GetAllPrograms(int pageNumber, int pageSize, string search, SortBy sortBy, SortType sortType, bool isDeleted);
        Task<Response> GetProgramById(Guid programId);
        public Task<Response> CreateProgram(CreateProgramRequest request);
        public Task<Response> UpdateProgram(Guid programId, UpdateProgramRequest request);
        public Task<Response> DeleteProgram(Guid programId);
    }
}
