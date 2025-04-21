using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;
using AcademicChatBot.Common.BussinessModel.Material;

namespace AcademicChatBot.Service.Contract
{
    public interface IMaterialService
    {
        Task<Response> CreateMaterial(CreateMaterialRequest request);
        Task<Response> DeleteMaterial(Guid materialId);
        Task<Response> GetAllMaterials(int pageNumber, int pageSize, string search);
        Task<Response> GetMaterialById(Guid materialId);
        Task<Response> UpdateMaterial(Guid materialId, UpdateMaterialRequest request);
    }
}
