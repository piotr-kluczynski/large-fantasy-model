using large_fantasy_model.Data;
using large_fantasy_model.Models;
using large_fantasy_model.ViewModels;
using large_fantasy_model.Hubs; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR; 
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace large_fantasy_model.Controllers
{
    [Authorize]
    public class FindGameController : Controller
    {
        private readonly LargeFantasyModelContext _context;
        private readonly IHubContext<LobbyHub> _lobbyHub; 

        public FindGameController(LargeFantasyModelContext context, IHubContext<LobbyHub> lobbyHub) 
        {
            _context = context;
            _lobbyHub = lobbyHub;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        [HttpGet]
        public async Task<IActionResult> Find(string searchQuery)
        {
            int myId = GetCurrentUserId();

            var query = _context.Games
                .Include(g => g.User)
                .Include(g => g.Users)
                .Where(g => g.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(g => g.Name.Contains(searchQuery));
            }

            var gamesList = await query.ToListAsync();

            var viewModel = new FindGameViewModel
            {
                SearchQuery = searchQuery,
                Games = gamesList.Select(g => new GameListItemViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    IsPublic = g.IsPublic,
                    HasPassword = !string.IsNullOrWhiteSpace(g.Password),
                    DungeonMasterName = g.User.Username,
                    PlayerCount = g.Users.Count + 1,
                    CreationDate = g.CreationDate,
                    IsAlreadyMember = g.UserId == myId || g.Users.Any(u => u.Id == myId),
                    MaxPlayers = g.MaxPlayers > 0 ? g.MaxPlayers : 10
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinCampaign(int gameId)
        {
            int myId = GetCurrentUserId();
            var game = await _context.Games.Include(g => g.Users).FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null) return NotFound();
            if (!game.IsActive) return BadRequest();

            int actualMaxPlayers = game.MaxPlayers > 0 ? game.MaxPlayers : 10;

            if (game.Users.Count + 1 >= actualMaxPlayers)
            {
                TempData["DangerMessage"] = "This Campaign is already full";
                return RedirectToAction("Find");
            }

            if (game.UserId != myId && !game.Users.Any(u => u.Id == myId))
            {
                var me = await _context.Users.FindAsync(myId);
                game.Users.Add(me);
                await _context.SaveChangesAsync();

                var characterName = await _context.Characters.Where(c => c.UserId == myId && c.Games.Any(g => g.Id == gameId)).Select(c => c.Name).FirstOrDefaultAsync();
                await _lobbyHub.Clients.Group($"Lobby_{gameId}").SendAsync("PlayerJoinedLobby", me.Id, me.Username, me.ProfilePicturePath, characterName);

                TempData["SuccessMessage"] = $"Successfully joined {game.Name}!";
            }

            return RedirectToAction("LobbyDetails", "Game", new { id = gameId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinCampaignWithPassword(int gameId, string password)
        {
            int myId = GetCurrentUserId();
            var game = await _context.Games.Include(g => g.Users).FirstOrDefaultAsync(g => g.Id == gameId);

            if (game == null) return NotFound();

            if (game.Password != password)
            {
                TempData["DangerMessage"] = "Incorrect password for this campaign.";
                return RedirectToAction("Find");
            }

            if (game.UserId != myId && !game.Users.Any(u => u.Id == myId))
            {
                var me = await _context.Users.FindAsync(myId);
                game.Users.Add(me);
                await _context.SaveChangesAsync();

                var characterName = await _context.Characters.Where(c => c.UserId == myId && c.Games.Any(g => g.Id == gameId)).Select(c => c.Name).FirstOrDefaultAsync();
                await _lobbyHub.Clients.Group($"Lobby_{gameId}").SendAsync("PlayerJoinedLobby", me.Id, me.Username, me.ProfilePicturePath, characterName);

                TempData["SuccessMessage"] = $"Successfully joined {game.Name}!";
            }

            return RedirectToAction("LobbyDetails", "Game", new { id = gameId });
        }

        [HttpGet]
        public async Task<IActionResult> GetGamesList(string searchQuery)
        {
            int myId = GetCurrentUserId();

            var query = _context.Games
                .Include(g => g.User)
                .Include(g => g.Users)
                .Where(g => g.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(g => g.Name.Contains(searchQuery));
            }

            var gamesList = await query.ToListAsync();

            var viewModel = new FindGameViewModel
            {
                Games = gamesList.Select(g => new GameListItemViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    IsPublic = g.IsPublic,
                    HasPassword = !string.IsNullOrWhiteSpace(g.Password),
                    DungeonMasterName = g.User.Username,
                    PlayerCount = g.Users.Count + 1,
                    CreationDate = g.CreationDate,
                    IsAlreadyMember = g.UserId == myId || g.Users.Any(u => u.Id == myId),
                    MaxPlayers = g.MaxPlayers > 0 ? g.MaxPlayers : 10
                }).ToList()
            };

            return PartialView("_GamesListPartial", viewModel);
        }
    }
}