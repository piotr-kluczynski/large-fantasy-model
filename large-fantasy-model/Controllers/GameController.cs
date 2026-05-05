using large_fantasy_model.Data;
using large_fantasy_model.Models;
using large_fantasy_model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace large_fantasy_model.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly LargeFantasyModelContext _context;

        public GameController(LargeFantasyModelContext context)
        {
            _context = context;
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
                JoinCode = generatedCode,
                Password = model.IsPublic ? null : model.Password 
            };

            _context.Games.Add(newGame);
            await _context.SaveChangesAsync();

            
            TempData["SuccessMessage"] = $"Campaign '{newGame.Name}' has been successfully created!";

            return RedirectToAction(nameof(Campaigns));
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

            TempData["DangerMessage"] = $"Campaign '{game.Name}' has been permanently deleted.";

            return RedirectToAction(nameof(Campaigns));
        }

        [HttpGet]
        public async Task<IActionResult> LobbyDetails(int id)
        {
            int myId = GetCurrentUserId();

            var game = await _context.Games
                .Include(g => g.User) 
                .Include(g => g.Users) 
                .FirstOrDefaultAsync(g => g.Id == id);

           
            if (game == null || (game.UserId != myId && !game.Users.Any(u => u.Id == myId)))
            {
                TempData["DangerMessage"] = "You don't have access to this lobby.";
                return RedirectToAction(nameof(Campaigns));
            }

            
            var me = await _context.Users.Include(u => u.Friends).Include(u => u.FriendOf).FirstOrDefaultAsync(u => u.Id == myId);
            var mutualFriends = me.Friends.Where(f => me.FriendOf.Any(fo => fo.Id == f.Id)).ToList();

            
            var friendsToInvite = mutualFriends.Where(f => !game.Users.Any(u => u.Id == f.Id) && game.UserId != f.Id).ToList();

            var viewModel = new GameLobbyViewModel
            {
                GameId = game.Id,
                Name = game.Name,
                Description = game.Description,
                JoinCode = game.JoinCode,
                IsPublic = game.IsPublic,
                IsDungeonMaster = game.UserId == myId,
                DungeonMaster = new UserViewModel { Id = game.User.Id, Username = game.User.Username },
                Players = game.Users.Select(u => new UserViewModel { Id = u.Id, Username = u.Username }).ToList(),
                AvailableFriends = friendsToInvite.Select(f => new UserViewModel { Id = f.Id, Username = f.Username }).ToList()
            };

            return View(viewModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return RedirectToAction(nameof(Campaigns));

            int myId = GetCurrentUserId();

           
            var game = await _context.Games.Include(g => g.Users).FirstOrDefaultAsync(g => g.JoinCode == code.ToUpper());

            if (game == null)
            {
                TempData["DangerMessage"] = "Invalid Join Code. The campaign might not exist.";
                return RedirectToAction(nameof(Campaigns));
            }

            
            if (game.UserId == myId || game.Users.Any(u => u.Id == myId))
            {
                TempData["SuccessMessage"] = "You are already a member of this campaign.";
                return RedirectToAction(nameof(LobbyDetails), new { id = game.Id });
            }

            var me = await _context.Users.FindAsync(myId);
            game.Users.Add(me);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Successfully joined '{game.Name}'!";
            return RedirectToAction(nameof(LobbyDetails), new { id = game.Id });
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InviteFriend(int gameId, int friendId)
        {
            int myId = GetCurrentUserId();
            var game = await _context.Games.Include(g => g.Users).FirstOrDefaultAsync(g => g.Id == gameId);

            if (game != null && (game.UserId == myId || game.Users.Any(u => u.Id == myId)))
            {
                var friend = await _context.Users.FindAsync(friendId);
                if (friend != null && !game.Users.Any(u => u.Id == friendId))
                {
                    game.Users.Add(friend);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"{friend.Username} has been added to the campaign.";
                }
            }
            return RedirectToAction(nameof(LobbyDetails), new { id = gameId });
        }
       
    }
}