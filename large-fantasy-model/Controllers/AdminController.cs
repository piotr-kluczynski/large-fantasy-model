using large_fantasy_model.Data;
using large_fantasy_model.Models;
using large_fantasy_model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace large_fantasy_model.Controllers
{
    [Authorize(Roles = "Admin")] // Tylko osoby z rolą Admin mają tu wstęp
    public class AdminController : Controller
    {
        private readonly LargeFantasyModelContext _context;

        public AdminController(LargeFantasyModelContext context)
        {
            _context = context;
        }

        // LISTA UŻYTKOWNIKÓW
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users
                .Select(u => new AdminUserViewModel
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    AdminPermissions = u.AdminPermissions,
                    CreatedDate = u.CreatedDate
                })
                .ToListAsync();

            return View(users);
        }

        // EDYCJA UŻYTKOWNIKA (GET)
        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            return View(user); // Przekazujemy model User do formularza edycji 
        }

        // EDYCJA UŻYTKOWNIKA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(User model)
        {
            var userInDb = await _context.Users.FindAsync(model.Id);
            if (userInDb == null) return NotFound();

            // Aktualizujemy dane
            userInDb.FirstName = model.FirstName;
            userInDb.LastName = model.LastName ?? "";
            userInDb.Email = model.Email;
            userInDb.AdminPermissions = model.AdminPermissions; // Tu możemy zmienić kogoś w Admina! 

            _context.Update(userInDb);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Users));
        }
    }
}