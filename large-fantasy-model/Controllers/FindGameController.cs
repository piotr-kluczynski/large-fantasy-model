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

        // --- GŁÓWNA TABLICA WYSZUKIWANIA GIER ---
        [HttpGet]
        public async Task<IActionResult> Find(string searchQuery)
        {
            int myId = GetCurrentUserId();

            var query = _context.Games
                .Include(g => g.User)
                .Include(g => g.Users)
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
    }
}