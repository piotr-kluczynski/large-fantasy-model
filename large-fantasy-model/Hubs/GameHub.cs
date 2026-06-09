using large_fantasy_model.Data;
using large_fantasy_model.Models;
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


            var userIdString = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userIdString))
            {
                int userId = int.Parse(userIdString);
                var character = await _context.Characters
                    .FirstOrDefaultAsync(c => c.UserId == userId && c.Games.Any(g => g.Id == gameId));
                if (character != null)
                {
                    username = $"{username} ({character.Name})";
                }
            }

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
        public async Task UpdateTokenHp(int gameId, int tokenId, int currentHp, int maxHp)
        {
            var token = await _context.Tokens.FindAsync(tokenId);
            if (token != null)
            {
                currentHp = Math.Max(0, Math.Min(currentHp, maxHp));

                token.CurrentHp = currentHp;
                token.MaxHp = maxHp;
                await _context.SaveChangesAsync();
            }

            string groupName = $"Game_{gameId}";
            await Clients.Group(groupName).SendAsync("TokenHpUpdated", tokenId, currentHp, maxHp);
        }
        public async Task ChangeMap(int gameId, string mapUrl)
        {
            var game = await _context.Games.FindAsync(gameId);
            if (game != null)
            {
                game.MapImageUrl = mapUrl;
                await _context.SaveChangesAsync();
            }

            string groupName = $"Game_{gameId}";
            await Clients.Group(groupName).SendAsync("MapChanged", mapUrl);
        }
        public async Task SpawnToken(int gameId, string name, int maxHp, string color, int x, int y)
        {
            try
            {

                var userIdString = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdString))
                {
                    await Clients.Caller.SendAsync("ReceiveGameMessage", "System", "[ERROR] SignalR lost user authentication context!", DateTime.Now.ToString("HH:mm"));
                    return;
                }

                int userId = int.Parse(userIdString);
                string groupName = $"Game_{gameId}";


                if (string.IsNullOrWhiteSpace(name))
                {
                    name = "Unknown Monster";
                }


                var token = new Token
                {
                    GameId = gameId,
                    Name = name,
                    MaxHp = maxHp,
                    CurrentHp = maxHp,
                    Color = color,
                    X = x,
                    Y = y,
                    UserId = userId
                };

                _context.Tokens.Add(token);
                await _context.SaveChangesAsync();

                await Clients.Group(groupName).SendAsync("TokenSpawned", token.Id, token.Name, maxHp, color, x, y, userId);
                await Clients.Group(groupName).SendAsync("ReceiveGameMessage", "System", $"[SYSTEM] {token.Name} has appeared on the battlefield!", DateTime.Now.ToString("HH:mm"));
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveGameMessage", "System", $"[ERROR] Failed to spawn: {ex.Message}", DateTime.Now.ToString("HH:mm"));
            }
        }
        public async Task DeleteToken(int gameId, int tokenId)
        {
            var userIdString = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return;

            int userId = int.Parse(userIdString);

            var token = await _context.Tokens.FindAsync(tokenId);
            if (token != null)
            {
                var game = await _context.Games.FindAsync(gameId);
                bool isDM = game != null && game.UserId == userId;

                if (isDM || token.UserId == userId)
                {
                    string tokenName = token.Name; 

                    _context.Tokens.Remove(token);
                    await _context.SaveChangesAsync();

                    string groupName = $"Game_{gameId}";

                    await Clients.Group(groupName).SendAsync("TokenDeleted", tokenId);

                    await Clients.Group(groupName).SendAsync("ReceiveGameMessage", "System", $"[SYSTEM] 💀 {tokenName} was removed from the map.", DateTime.Now.ToString("HH:mm"));
                }
            }
        }
        public async Task UpdateInitiative(int gameId, string initiativeJson)
        {
            string groupName = $"Game_{gameId}";
            await Clients.Group(groupName).SendAsync("InitiativeUpdated", initiativeJson);
        }

        public async Task SetActiveTurn(int gameId, int? tokenId)
        {
            string groupName = $"Game_{gameId}";
            await Clients.Group(groupName).SendAsync("ActiveTurnChanged", tokenId);
        }
    }
}