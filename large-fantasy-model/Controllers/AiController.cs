using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace large_fantasy_model.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : Controller
    {
        private readonly IConfiguration _config;
        public AiController(IConfiguration config) => _config = config;

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateLore([FromBody] string prompt)
        {
            var apiKey = _config["Gemini:ApiKey"];
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3-flash-preview:generateContent?key={apiKey}";

            using var client = new HttpClient();
            var requestBody = new
            {
                contents = new[] { new { parts = new[] { new { text = "You are an expert RPG Dungeon Master assistant. Help write epic, detailed, and atmospheric Campaign Lore. Always respond in the exact same language that the user uses in their prompt. User's prompt: " + prompt } } } }
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
