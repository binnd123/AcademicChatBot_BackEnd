using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademicChatBot.Common.BussinessModel.Messages;
using AcademicChatBot.Common.Enum;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.SignalR;

namespace AcademicChatBot.Service.HubService
{
    public class HubService : IHubService
    {
        public readonly IHubContext<ChatHub> _signalRHub;

        public HubService(IHubContext<ChatHub> signalRHub)
        {
            _signalRHub = signalRHub;
        }

        public async Task SendMessageToUserAsync(Guid receiverUserId, string method, object data)
        {
            // Kiểm tra tin nhắn không được rỗng
            if (data == null)
            {
                await _signalRHub.Clients.Group(receiverUserId.ToString()).SendAsync("ReceiveMessage", "ChatBot", "Tin nhắn không hợp lệ.");
                return;
            }
            await _signalRHub.Clients.Group(receiverUserId.ToString())
            .SendAsync(method, data);
            ///////////////////////////////////////

            
        }
    }
}
