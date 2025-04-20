using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicChatBot.Service.HubService
{
    public interface IHubService
    {
        Task SendMessageToUserAsync(Guid receiverUserId, string method, object data);
    }
}
