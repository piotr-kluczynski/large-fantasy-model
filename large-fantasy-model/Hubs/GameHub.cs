using Microsoft.AspNetCore.SignalR;

namespace large_fantasy_model.Hubs
{
    public class GameHub : Hub
    {
        public async Task JoinGameSession(int gameId, string username)
        {
            string groupName = $"Game_{gameId}";
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.OthersInGroup(groupName).SendAsync("UserStatusChanged", Context.UserIdentifier, true, username);

            await Clients.OthersInGroup(groupName).SendAsync("RequestStatusSync");
        }

        public async Task ReportStatus(int gameId, string userId)
        {
            await Clients.Group($"Game_{gameId}").SendAsync("UserStatusChanged", userId, true, "");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {

            await base.OnDisconnectedAsync(exception);
        }

        public async Task LeaveGameSession(int gameId)
        {
            string groupName = $"Game_{gameId}";
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.OthersInGroup(groupName).SendAsync("UserStatusChanged", Context.UserIdentifier, false, "");
        }

        public async Task SendMessageToGame(int gameId, string username, string message)
        {
            string groupName = $"Game_{gameId}";
            string timeString = DateTime.Now.ToString("HH:mm");

            await Clients.Group(groupName).SendAsync("ReceiveGameMessage", username, message, timeString);
        }
    }
}