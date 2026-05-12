using Microsoft.AspNetCore.SignalR;

namespace large_fantasy_model.Hubs
{
    public class LobbyHub : Hub
    {
        // Ten hub będzie służył do powiadamiania wszystkich o zmianach w liście lobby
        public async Task BroadcastLobbyUpdate()
        {
            await Clients.All.SendAsync("RefreshLobbyList");
        }
    }
}