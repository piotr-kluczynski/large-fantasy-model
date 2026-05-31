using large_fantasy_model.Data;
using large_fantasy_model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace large_fantasy_model.Controllers
{
    public class AiRequest
    {
        public string Prompt { get; set; }
        public int? GameId { get; set; }
        public bool IsInGameAssistant { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AiController : Controller
    {
        private readonly IConfiguration _config;
        private readonly LargeFantasyModelContext _context;

        public AiController(IConfiguration config, LargeFantasyModelContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateContent([FromBody] AiRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt)) return BadRequest("Prompt is empty");

            Game game = null;
            if (request.GameId.HasValue)
            {
                game = await _context.Games.Include(g => g.Users).FirstOrDefaultAsync(g => g.Id == request.GameId.Value);
            }

            string systemPrompt = "";

            if (request.IsInGameAssistant)
            {
                string loreContext = game != null && !string.IsNullOrWhiteSpace(game.Lore) 
                    ? $"The current campaign lore is as follows:\n\n{game.Lore}\n\nAnswer the Dungeon Master's questions based on this lore." 
                    : "The campaign lore is currently empty.";

                systemPrompt = $"You are an in-game AI Assistant for the Dungeon Master. {loreContext} Always respond in the exact same language that the user uses in their prompt. User's prompt: {request.Prompt}";
            }
            else
            {
                int playerCount = game != null ? game.Users.Count + 1 : 1;
                systemPrompt = $"You are an expert RPG Dungeon Master assistant. Help write epic, detailed, and atmospheric Campaign Lore. There are currently {playerCount} players in this game. Keep this in mind when generating scenarios or roles. Always respond in the exact same language that the user uses in their prompt. Do NOT include any conversational filler, introductory remarks, or concluding statements (e.g., 'Oto fundamenty...', 'Here is the lore...', 'Enjoy your campaign!'). Return ONLY the pure campaign lore text formatted in Markdown without any additional comments. User's prompt: {request.Prompt}";
            }

            var apiKey = _config["Gemini:ApiKey"];
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3-flash-preview:generateContent?key={apiKey}";

            using var client = new HttpClient();
            var requestBody = new
            {
                contents = new[] { new { parts = new[] { new { text = systemPrompt } } } }
            };

            var response = await client.PostAsJsonAsync(url, requestBody);

            if (!response.IsSuccessStatusCode)
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                return BadRequest($"Request rejected. Status: {response.StatusCode}. Szczegóły: {errorDetails}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            var text = doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();

            return Ok(new { response = text });
        }
    }
}
