using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using large_fantasy_model.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace large_fantasy_model.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiCharactersController : ControllerBase
    {
        private readonly LargeFantasyModelContext _context;

        public ApiCharactersController(LargeFantasyModelContext context)
        {
            _context = context;
        }

        // GET: api/ApiCharacters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCharacters()
        {
            return await _context.Characters
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

        // GET: api/ApiCharacters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetCharacter(int id)
        {
            var character = await _context.Characters
                .Where(c => c.Id == id)
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
