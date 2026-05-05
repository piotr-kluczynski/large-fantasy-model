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

            
            var newGame = new Game
            {
                Name = model.Name,
                Description = model.Description,
                CreationDate = DateTime.Now,
                LastSessionDate = DateTime.Now,
                UserId = myId, 
                ConversationId = gameConversation.Id
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
    }
}