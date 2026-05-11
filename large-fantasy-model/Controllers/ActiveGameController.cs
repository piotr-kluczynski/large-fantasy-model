using large_fantasy_model.Data;
using large_fantasy_model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace large_fantasy_model.Controllers
{
    [Authorize]
    public class ActiveGameController : Controller
    {
        private readonly LargeFantasyModelContext _context;

        public ActiveGameController(LargeFantasyModelContext context)
        {
            _context = context;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            int myId = GetCurrentUserId();

            var game = await _context.Games
                .Include(g => g.User) 
                .Include(g => g.Users) 
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null || (game.UserId != myId && !game.Users.Any(u => u.Id == myId)))
            {
                TempData["DangerMessage"] = "Nie masz dostępu do tej sesji gry.";
                return RedirectToAction("Campaigns", "Game"); 
            }

            var viewModel = new GameLobbyViewModel
            {
                GameId = game.Id,
                Name = game.Name,
                IsDungeonMaster = game.UserId == myId,
                DungeonMaster = new UserViewModel { Id = game.User.Id, Username = game.User.Username },
                Players = game.Users.Select(u => new UserViewModel { Id = u.Id, Username = u.Username }).ToList()
            };

            return View(viewModel);
        }
    }
}