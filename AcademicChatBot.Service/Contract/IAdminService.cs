using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel;

namespace AcademicChatBot.Service.Contract
{
    public interface IAdminService
    {
        public Task<Response> CreateAdminIfNotExistsAsync();
        public Task<Response> GetReportsAsync();
        public Task<Response> SetUserActiveStatusAsync(Guid userId, bool isActive);
        public Task<Response> DeleteUser(Guid userId);
    }
}
