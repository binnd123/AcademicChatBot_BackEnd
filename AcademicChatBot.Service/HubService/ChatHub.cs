using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace AcademicChatBot.Service.HubService
{
    public class ChatHub : Hub
    {
        // Tham gia vào nhóm của user để có thể nhận tin nhắn
        public async Task JoinUserGroup(Guid userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
        }

        public async Task LeaveUserGroup(Guid userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId.ToString());
        }
    }
}
