using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using large_fantasy_model.Data;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace large_fantasy_model.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApiCharactersController : ControllerBase
    {
        private readonly LargeFantasyModelContext _context;

        public ApiCharactersController(LargeFantasyModelContext context)
        {
            _context = context;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCharacters()
        {
            bool isHeadAdmin = User.IsInRole("HeadAdmin");
            int myId = GetCurrentUserId();

            return await _context.Characters
                .Where(c => isHeadAdmin || c.UserId == myId)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Level,
                    c.Xp,
                    c.Alignment,
                    c.ArmorClass,
                    c.MaxHitPoints,
                    UserId = c.UserId
                })
                .ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetCharacter(int id)
        {
            bool isHeadAdmin = User.IsInRole("HeadAdmin");
            int myId = GetCurrentUserId();

            var character = await _context.Characters
                .Where(c => c.Id == id && (isHeadAdmin || c.UserId == myId))
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Level,
                    c.Xp,
                    c.Alignment,
                    c.ArmorClass,
                    c.MaxHitPoints,
                    UserId = c.UserId
                })
                .FirstOrDefaultAsync();

            if (character == null)
            {
                return NotFound();
            }

            return character;
        }
    }
}
