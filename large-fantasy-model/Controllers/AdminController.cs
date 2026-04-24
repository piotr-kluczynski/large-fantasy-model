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
                    CreatedDate = u.CreatedDate,
                    LockoutEnd = u.LockoutEnd
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
        // USUWANIE UŻYTKOWNIKA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            // Opcjonalnie: Zabezpieczenie, żeby admin nie mógł usunąć samego siebie
            var currentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (id == currentUserId)
            {
                TempData["Error"] = "You cannot delete your own admin account!";
                return RedirectToAction(nameof(Users));
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "User has been deleted successfully.";
            return RedirectToAction(nameof(Users));
        }
        // DODAWANIE UŻYTKOWNIKA (GET)
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View(new User()); // Przesyłamy pusty model
        }

        // DODAWANIE UŻYTKOWNIKA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(User user)
        {
            // Usuwamy walidację Id, bo baza sama je wygeneruje
            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                // Sprawdzenie czy mail/login zajęty
                if (_context.Users.Any(u => u.Email == user.Email || u.Username == user.Username))
                {
                    ModelState.AddModelError("", "Użytkownik z tym mailem lub loginem już istnieje.");
                    return View(user);
                }

                user.CreatedDate = DateTime.Now;
                user.LastName = user.LastName ?? ""; // Zabezpieczenie przed nullem

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"User {user.Username} created successfully!";
                return RedirectToAction(nameof(Users));
            }

            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> LockUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUser(int id, int days, string reason)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.LockoutEnd = DateTime.Now.AddDays(days);
            user.LockoutReason = reason;

            _context.Update(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"User {user.Username} has been locked for {days} days.";
            return RedirectToAction(nameof(Users));
        }

        // Dodatkowo: Odblokowywanie
        [HttpPost]
        public async Task<IActionResult> UnlockUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.LockoutEnd = null;
            user.LockoutReason = null;

            _context.Update(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Account unlocked.";
            return RedirectToAction(nameof(Users));
        }
    }
}