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
    public class FindGameController : Controller
    {
        private readonly LargeFantasyModelContext _context;

        public FindGameController(LargeFantasyModelContext context)
        {
            _context = context;
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
                    IsAlreadyMember = g.UserId == myId || g.Users.Any(u => u.Id == myId)
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

            if (game.Users.Count + 1 >= 10)
            {
                TempData["DangerMessage"] = "This Campaigne is already full";
                return RedirectToAction("Find"); 
            }
            

           
            if (!game.Users.Any(u => u.Id == myId) && game.UserId != myId)
            {
                var user = await _context.Users.FindAsync(myId);
                game.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            
            if (!game.IsActive)
            {
                TempData["DangerMessage"] = "This campaign is currently inactive.";
                return RedirectToAction("Find");
            }

            if (game.UserId != myId && !game.Users.Any(u => u.Id == myId))
            {
                var me = await _context.Users.FindAsync(myId);
                game.Users.Add(me);
                await _context.SaveChangesAsync();
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
                TempData["SuccessMessage"] = $"Successfully joined {game.Name}!";
            }

            return RedirectToAction("LobbyDetails", "Game", new { id = gameId });
        }
    }
}