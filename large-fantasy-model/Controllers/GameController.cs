using large_fantasy_model.Data;
using large_fantasy_model.Hubs;
using large_fantasy_model.Migrations;
using large_fantasy_model.Models;
using large_fantasy_model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace large_fantasy_model.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly LargeFantasyModelContext _context;
        private readonly IHubContext<LobbyHub> _lobbyHub;
        private readonly IHubContext<PrivateMessageHub> _privateMessageHub; 

        public GameController(LargeFantasyModelContext context, IHubContext<LobbyHub> lobbyHub, IHubContext<PrivateMessageHub> privateMessageHub)
        {
            _context = context;
            _lobbyHub = lobbyHub;
            _privateMessageHub = privateMessageHub;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        [HttpGet]
        public async Task<IActionResult> Campaigns()
        {
            int myId = GetCurrentUserId();

            var myGames = await _context.Games
                .Include(g => g.User)
                .Include(g => g.Users)
                .Where(g => g.UserId == myId || g.Users.Any(u => u.Id == myId))
                .ToListAsync();

            return View(myGames);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateGameViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameViewModel model)
        {
            if (!model.IsPublic && string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("Password", "Password is Required for private campaigns.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int myId = GetCurrentUserId();

            var gameConversation = new Conversation
            {
                Title = $"{model.Name} - Campaign Chat"
            };

            _context.Conversations.Add(gameConversation);
            await _context.SaveChangesAsync();

            string generatedCode = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

            var newGame = new Game
            {
                Name = model.Name,
                Description = model.Description,
                CreationDate = DateTime.Now,
                LastSessionDate = DateTime.Now,
                UserId = myId,
                ConversationId = gameConversation.Id,
                IsPublic = model.IsPublic,
                MaxPlayers = model.MaxPlayers,
                IsActive = true,
                JoinCode = generatedCode,
                Password = model.IsPublic ? null : model.Password
            };

            _context.Games.Add(newGame);
            await _context.SaveChangesAsync();


            await _lobbyHub.Clients.All.SendAsync("RefreshLobbyList");
            TempData["SuccessMessage"] = $"Campaign '{newGame.Name}' has been successfully created!";
            return RedirectToAction(nameof(Campaigns));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            int myId = GetCurrentUserId();
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id && g.UserId == myId);

            if (game == null) return NotFound();

            var model = new CreateGameViewModel
            {
                Name = game.Name,
                Description = game.Description,
                IsPublic = game.IsPublic,
                Password = game.Password,
                MaxPlayers = game.MaxPlayers
            };

            ViewBag.GameId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateGameViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            int myId = GetCurrentUserId();
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id && g.UserId == myId);

            if (game == null) return NotFound();

            game.Name = model.Name;
            game.Description = model.Description;
            game.IsPublic = model.IsPublic;
            game.MaxPlayers = model.MaxPlayers;
            game.Password = model.IsPublic ? null : model.Password;

            _context.Update(game);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Campaign settings updated successfully!";
            return RedirectToAction(nameof(LobbyDetails), new { id = game.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            int myId = GetCurrentUserId();

            var game = await _context.Games
                .Include(g => g.Conversation)
                .FirstOrDefaultAsync(g => g.Id == id && g.UserId == myId);

            if (game == null)
            {
                TempData["DangerMessage"] = "You don't have permission to delete this campaign or it doesn't exist.";
                return RedirectToAction(nameof(Campaigns));
            }

            if (game.Conversation != null)
            {
                _context.Conversations.Remove(game.Conversation);
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            await _lobbyHub.Clients.All.SendAsync("RefreshLobbyList");

            TempData["DangerMessage"] = $"Campaign '{game.Name}' has been permanently deleted.";
            return RedirectToAction(nameof(Campaigns));
        }

        [HttpGet]
        public async Task<IActionResult> LobbyDetails(int id, string tab = "main")
        {
            int myId = GetCurrentUserId();

            var game = await _context.Games
                .Include(g => g.User)
                .Include(g => g.Users)
                .Include(g => g.Characters)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null || (game.UserId != myId && !game.Users.Any(u => u.Id == myId)))
            {
                TempData["DangerMessage"] = "You don't have access to this lobby.";
                return RedirectToAction(nameof(Campaigns));
            }

            var me = await _context.Users.Include(u => u.Friends).Include(u => u.FriendOf).FirstOrDefaultAsync(u => u.Id == myId);
            var mutualFriends = me.Friends.Where(f => me.FriendOf.Any(fo => fo.Id == f.Id)).ToList();
            var friendsToInvite = mutualFriends.Where(f => !game.Users.Any(u => u.Id == f.Id) && game.UserId != f.Id).ToList();

           
            var invitedIds = await _context.Notifications
                .Where(n => n.RelatedEntityId == id && n.Type == "GameInvite")
                .Select(n => n.ReceiverId)
                .ToListAsync();

            var playerCharacters = new Dictionary<int, string>();
            foreach (var character in game.Characters)
            {
                playerCharacters[character.UserId] = character.Name;
            }

            var availableCharacters = await _context.Characters.Include(c => c.Class).Where(c => c.UserId == myId).ToListAsync();
            var currentCharacter = game.Characters.FirstOrDefault(c => c.UserId == myId);

            var viewModel = new GameLobbyViewModel
            {
                GameId = game.Id,
                Name = game.Name,
                Description = game.Description,
                JoinCode = game.JoinCode,
                IsPublic = game.IsPublic,
                IsDungeonMaster = game.UserId == myId,
                IsActive = game.IsActive,
                DungeonMaster = new UserViewModel { Id = game.User.Id, Username = game.User.Username, ProfilePicturePath = game.User.ProfilePicturePath },
                Players = game.Users.Select(u => new UserViewModel { Id = u.Id, Username = u.Username, ProfilePicturePath = u.ProfilePicturePath }).ToList(),
                AvailableFriends = friendsToInvite.Select(f => new UserViewModel { Id = f.Id, Username = f.Username, ProfilePicturePath = f.ProfilePicturePath }).ToList(),
                CurrentPlayers = game.Users.Count + 1,
                MaxPlayers = game.MaxPlayers > 0 ? game.MaxPlayers : 10,
                InvitedFriendIds = invitedIds,
                Lore = game.Lore,
                PlayerCharacters = playerCharacters,
                AvailableCharacters = availableCharacters,
                CurrentUserSelectedCharacterId = currentCharacter?.Id
            };

            ViewBag.ActiveTab = tab;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            int myId = GetCurrentUserId();
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id && g.UserId == myId);

            if (game == null) return NotFound();

            game.IsActive = !game.IsActive;
            await _context.SaveChangesAsync();

            string status = game.IsActive ? "visible" : "hidden";
            TempData["SuccessMessage"] = $"Campaign '{game.Name}' is now {status}.";
            await _lobbyHub.Clients.All.SendAsync("RefreshLobbyList");

            return RedirectToAction(nameof(LobbyDetails), new { id = game.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InviteFriend(int gameId, int friendId)
        {
            int myId = GetCurrentUserId();
            var game = await _context.Games.Include(g => g.Users).FirstOrDefaultAsync(g => g.Id == gameId && g.UserId == myId);

            if (game == null) return Unauthorized();

            var friendToInvite = await _context.Users.FindAsync(friendId);
            if (friendToInvite == null) return NotFound();

            if (game.Users.Any(u => u.Id == friendId))
            {
                TempData["DangerMessage"] = $"{friendToInvite.Username} is already in your party.";
                return RedirectToAction("LobbyDetails", new { id = gameId });
            }

            var existingInvite = await _context.Notifications
                .AnyAsync(n => n.ReceiverId == friendId && n.Type == "GameInvite" && n.RelatedEntityId == gameId);

            if (existingInvite)
            {
                TempData["DangerMessage"] = $"An invitation has already been sent to {friendToInvite.Username}.";
                return RedirectToAction("LobbyDetails", new { id = gameId });
            }

            var notification = new Notification
            {
                ReceiverId = friendId,
                SenderId = myId,
                Type = "GameInvite",
                Message = $"invited you to join the campaign: {game.Name}",
                RelatedEntityId = gameId
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            
            await _privateMessageHub.Clients.Group($"User_{friendId}").SendAsync("UpdateNotifications");

            TempData["SuccessMessage"] = $"Invitation sent to {friendToInvite.Username}!";
            return RedirectToAction("LobbyDetails", new { id = gameId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptGameInvite(int notificationId)
        {
            int myId = GetCurrentUserId();
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.ReceiverId == myId);

            if (notification == null)
            {
                TempData["DangerMessage"] = "Invitation not found.";
                return RedirectToAction("Campaigns");
            }

            if (!notification.RelatedEntityId.HasValue)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
                TempData["DangerMessage"] = "Invitation error: Missing game ID.";
                return RedirectToAction("Campaigns");
            }

            int gameId = notification.RelatedEntityId.Value;
            var game = await _context.Games
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
                TempData["DangerMessage"] = "This campaign no longer exists (it was probably deleted).";
                return RedirectToAction("Campaigns");
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

         
            await _privateMessageHub.Clients.Group($"User_{myId}").SendAsync("UpdateNotifications");

            if (game.UserId == myId)
            {
                TempData["DangerMessage"] = "You are already the Dungeon Master of this campaign.";
                return RedirectToAction("LobbyDetails", new { id = gameId });
            }

            int actualMaxPlayers = game.MaxPlayers > 0 ? game.MaxPlayers : 10;

            if (game.Users.Count + 1 >= actualMaxPlayers)
            {
                TempData["DangerMessage"] = "This campaign is already full.";
                return RedirectToAction("Campaigns");
            }

            if (!game.Users.Any(u => u.Id == myId))
            {
                var me = await _context.Users.FirstOrDefaultAsync(u => u.Id == myId);
                if (me != null)
                {
                    game.Users.Add(me);
                    await _context.SaveChangesAsync();
                    await _lobbyHub.Clients.Group($"Lobby_{gameId}").SendAsync("PlayerJoinedLobby", me.Id, me.Username);

                    TempData["SuccessMessage"] = $"Successfully joined {game.Name}!";
                    return RedirectToAction("LobbyDetails", new { id = gameId });
                }
            }

            TempData["SuccessMessage"] = "You are already in this campaign!";
            return RedirectToAction("LobbyDetails", new { id = gameId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectNotification(int notificationId)
        {
            int myId = GetCurrentUserId();
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.ReceiverId == myId);

            if (notification != null)
            {
                string type = notification.Type;
                int? gameId = notification.RelatedEntityId;

                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();

    
                await _privateMessageHub.Clients.Group($"User_{myId}").SendAsync("UpdateNotifications");

                if (type == "GameInvite" && gameId.HasValue)
                {
                    await _lobbyHub.Clients.Group($"Lobby_{gameId.Value}").SendAsync("GameInviteDeclined", myId);
                }

                TempData["SuccessMessage"] = "Zaproszenie zostało odrzucone.";
            }

            string referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePlayer(int gameId, int playerId)
        {
            int myId = GetCurrentUserId();
            var game = await _context.Games.Include(g => g.Users).FirstOrDefaultAsync(g => g.Id == gameId && g.UserId == myId);

            if (game == null) return Unauthorized();

            var playerToRemove = game.Users.FirstOrDefault(u => u.Id == playerId);
            if (playerToRemove != null)
            {
                string username = playerToRemove.Username;
                game.Users.Remove(playerToRemove);
                await _context.SaveChangesAsync();

                await _lobbyHub.Clients.Group($"Lobby_{gameId}").SendAsync("PlayerLeftLobby", playerId, username);

                TempData["SuccessMessage"] = $"Player {username} has been removed from the party.";
            }

            return RedirectToAction("LobbyDetails", new { id = gameId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            int myId = GetCurrentUserId();
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.ReceiverId == myId);

            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();

                await _privateMessageHub.Clients.Group($"User_{myId}").SendAsync("UpdateNotifications");
            }

            string referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LeaveCampaign(int gameId)
        {
            int myId = GetCurrentUserId();
            var game = await _context.Games
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null) return NotFound();

            if (game.UserId == myId)
            {
                TempData["DangerMessage"] = "The owner cannot leave the campaign. You can only delete it.";
                return RedirectToAction("LobbyDetails", new { id = gameId });
            }

            var me = game.Users.FirstOrDefault(u => u.Id == myId);
            if (me != null)
            {
                string username = me.Username;
                game.Users.Remove(me);
                await _context.SaveChangesAsync();

                await _lobbyHub.Clients.Group($"Lobby_{gameId}").SendAsync("PlayerLeftLobby", myId, username);

                TempData["SuccessMessage"] = $"You have left the campaign {game.Name}.";
            }

            return RedirectToAction(nameof(Campaigns));
        }

        [HttpGet]
        public IActionResult GetNotificationBell()
        {
            return ViewComponent("NotificationBell");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveLore(int gameId, string lore)
        {
            int myId = GetCurrentUserId();
   
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == gameId && g.UserId == myId);

            if (game == null) return Unauthorized();

            game.Lore = lore;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Campaign lore has been updated successfully!";
            return RedirectToAction(nameof(LobbyDetails), new { id = gameId, tab = "lore" });
        }

        [HttpGet]
        public async Task<IActionResult> DownloadLore(int gameId)
        {
            int myId = GetCurrentUserId();
            var game = await _context.Games.Include(g => g.Users).FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null || (game.UserId != myId && !game.Users.Any(u => u.Id == myId)))
            {
                return Unauthorized();
            }

            string loreText = game.Lore ?? "The chronicles are currently empty.";
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(loreText);
            string fileName = $"{game.Name.Replace(" ", "_")}_Lore.txt";

            return File(fileBytes, "text/plain", fileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectCharacter(int gameId, int? characterId)
        {
            int myId = GetCurrentUserId();
            var game = await _context.Games
                .Include(g => g.Users)
                .Include(g => g.Characters)
                .FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null) return NotFound();

            if (game.UserId != myId && !game.Users.Any(u => u.Id == myId))
            {
                return Unauthorized();
            }

            var existingCharacter = game.Characters.FirstOrDefault(c => c.UserId == myId);
            if (existingCharacter != null)
            {
                game.Characters.Remove(existingCharacter);
            }

            if (characterId.HasValue)
            {
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == characterId.Value && c.UserId == myId);
                if (character != null)
                {
                    game.Characters.Add(character);
                    TempData["SuccessMessage"] = $"Selected character: {character.Name}!";
                    await _lobbyHub.Clients.Group($"Lobby_{gameId}").SendAsync("PlayerSelectedCharacter", myId, character.Name);
                }
            }
            else
            {
                TempData["SuccessMessage"] = "Character selection cleared.";
                await _lobbyHub.Clients.Group($"Lobby_{gameId}").SendAsync("PlayerClearedCharacter", myId);
            }

            await _context.SaveChangesAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, hasCharacter = characterId.HasValue });
            }

            return RedirectToAction(nameof(LobbyDetails), new { id = gameId });
        }
    }
}