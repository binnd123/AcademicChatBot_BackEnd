using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;

namespace AcademicChatBot.Service.Contract
{
    public interface IPOMappingPLOService
    {
        public Task<Response> GetAllPOsForPLO(Guid PLOId, int pageNumber, int pageSize);
        public Task<Response> GetAllPLOsForPO(Guid POId, int pageNumber, int pageSize);
        public Task<Response> GetPOMappingPLOById(Guid pOMappingPLOId);
        public Task<Response> AddPLOsToPO(Guid pOId, List<Guid> pLOIds);
        public Task<Response> AddPOsToPLO(Guid pLOId, List<Guid> pOIds);
        public Task<Response> DeletePLOsFromPO(Guid pOId, List<Guid> pLOIds);
        public Task<Response> DeletePOsFromPLO(Guid pLOId, List<Guid> pOIds);
        public Task<Response> DeleteAllPLOsFromPO(Guid pOId);
        public Task<Response> DeleteAllPOsFromPLO(Guid pLOId);
    }
}
