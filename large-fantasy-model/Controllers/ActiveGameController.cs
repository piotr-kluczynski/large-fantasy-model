using large_fantasy_model.Data;
using large_fantasy_model.ViewModels;
using large_fantasy_model.Models; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq; 
using System;

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


            var existingTokens = await _context.Tokens.Where(t => t.GameId == id).ToListAsync();

            if (!existingTokens.Any(t => t.UserId == game.User.Id))
            {
                var dmToken = new Token { GameId = id, UserId = game.User.Id, Name = game.User.Username, Color = "#ffc107", X = 500, Y = 500, CurrentHp = 100, MaxHp = 100 };
                _context.Tokens.Add(dmToken);
                existingTokens.Add(dmToken);
            }

            foreach (var player in game.Users)
            {
                if (!existingTokens.Any(t => t.UserId == player.Id))
                {
                    var rand = new Random();
                    int offsetX = rand.Next(-2, 3) * 50;
                    int offsetY = rand.Next(-2, 3) * 50;

                    var playerToken = new Token { GameId = id, UserId = player.Id, Name = player.Username, Color = "#0d6efd", X = 500 + offsetX, Y = 500 + offsetY, CurrentHp = 100, MaxHp = 100 };
                    _context.Tokens.Add(playerToken);
                    existingTokens.Add(playerToken);
                }
            }
            await _context.SaveChangesAsync();
            var chatHistory = await _context.GameChatMessages
                .Where(m => m.GameId == id)
                .OrderBy(m => m.Timestamp)
                .Take(50)
                .ToListAsync();

            var viewModel = new ActiveGameViewModel
            {
                GameId = game.Id,
                Name = game.Name,
                IsDungeonMaster = game.UserId == myId,
                DungeonMaster = new UserViewModel { Id = game.User.Id, Username = game.User.Username, ProfilePicturePath = game.User.ProfilePicturePath },
                Players = game.Users.Select(u => new UserViewModel { Id = u.Id, Username = u.Username, ProfilePicturePath = u.ProfilePicturePath }).ToList(),
                MapImageUrl = game.MapImageUrl, 

                Tokens = existingTokens.Select(t => new TokenViewModel
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    Name = t.Name,
                    X = t.X,
                    Y = t.Y,
                    Color = t.Color,
                    CurrentHp = t.CurrentHp,
                    MaxHp = t.MaxHp
                }).ToList(),

                ChatMessages = chatHistory.Select(m => new ChatMessageViewModel
                {
                    SenderName = m.SenderName,
                    Text = m.Text,
                    Time = m.Timestamp.ToString("HH:mm")
                }).ToList()
            };

            return View(viewModel);
        }
    }
}