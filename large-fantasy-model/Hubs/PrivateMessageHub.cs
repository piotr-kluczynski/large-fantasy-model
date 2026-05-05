using Microsoft.AspNetCore.SignalR;

namespace large_fantasy_model.Hubs
{
    public class PrivateMessageHub : Hub
    {
        
        public async Task JoinMyUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
        }
    }
}