using large_fantasy_model.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace large_fantasy_model.Hubs
{
    public class GameHub : Hub
    {
        private readonly LargeFantasyModelContext _context;

        public GameHub(LargeFantasyModelContext context)
        {
            _context = context;
        }

        public async Task JoinGameSession(int gameId, string username)
        {
            string groupName = $"Game_{gameId}";
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.OthersInGroup(groupName).SendAsync("UserStatusChanged", Context.UserIdentifier, true, username);
            await Clients.OthersInGroup(groupName).SendAsync("RequestStatusSync");
        }

        public async Task ReportStatus(int gameId, string userId)
        {
            string groupName = $"Game_{gameId}";
            await Clients.Group(groupName).SendAsync("UserStatusChanged", userId, true, "");
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
            var now = DateTime.Now;
            string timeString = now.ToString("HH:mm");


            var chatMessage = new Models.GameChatMessage
            {
                GameId = gameId,
                SenderName = username,
                Text = message,
                Timestamp = now
            };

            _context.GameChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            await Clients.Group(groupName).SendAsync("ReceiveGameMessage", username, message, timeString);
        }


        public async Task UpdateTokenPosition(int gameId, int tokenId, int x, int y)
        {
            var token = await _context.Tokens.FindAsync(tokenId);
            if (token != null)
            {
                token.X = x;
                token.Y = y;
                await _context.SaveChangesAsync();
            }

            string groupName = $"Game_{gameId}";

            await Clients.OthersInGroup(groupName).SendAsync("TokenMoved", tokenId, x, y);
        }
    }
}