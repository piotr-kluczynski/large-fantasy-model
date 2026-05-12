using Microsoft.AspNetCore.SignalR;

namespace large_fantasy_model.Hubs
{
    public class LobbyHub : Hub
    {
        public async Task BroadcastLobbyUpdate()
        {
            await Clients.All.SendAsync("RefreshLobbyList");
        }


        public async Task JoinLobbyGroup(int gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Lobby_{gameId}");
        }


        public async Task LeaveLobbyGroup(int gameId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Lobby_{gameId}");
        }
    }
}