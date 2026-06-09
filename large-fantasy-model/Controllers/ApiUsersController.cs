using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using large_fantasy_model.Data;
using large_fantasy_model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace large_fantasy_model.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiUsersController : ControllerBase
    {
        private readonly LargeFantasyModelContext _context;

        public ApiUsersController(LargeFantasyModelContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            return await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.FirstName,
                    u.LastName,
                    u.CreatedDate,
                    u.Bio
                })
                .ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUser(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.FirstName,
                    u.LastName,
                    u.CreatedDate,
                    u.Bio
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
    }
}
