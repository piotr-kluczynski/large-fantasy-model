using large_fantasy_model.Data;
using large_fantasy_model.Models;
using large_fantasy_model.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace large_fantasy_model.Controllers
{
    [Authorize(Roles = "Admin,HeadAdmin")] // Dostęp dla obu rang
    public class AdminController : Controller
    {
        private readonly LargeFantasyModelContext _context;

        public AdminController(LargeFantasyModelContext context)
        {
            _context = context;
        }

        // Pomocnicza metoda sprawdzająca hierarchię
        private bool CanManage(int myPermissions, int targetPermissions)
        {
            // HeadAdmin (2) zarządza wszystkimi prócz siebie
            if (myPermissions == 2) return true;
            // Admin (1) zarządza tylko zwykłymi Userami (0)
            if (myPermissions == 1 && targetPermissions == 0) return true;

            return false;
        }

        private int GetMyPermissions()
        {
            var claim = User.FindFirst("AdminPermissions")?.Value;
            return int.TryParse(claim, out int res) ? res : 0;
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

            if (!CanManage(GetMyPermissions(), user.AdminPermissions))
            {
                TempData["Error"] = "You do not have permission to edit this user.";
                return RedirectToAction(nameof(Users));
            }

            return View(user);
        }

        // EDYCJA UŻYTKOWNIKA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(User model)
        {
            var userInDb = await _context.Users.FindAsync(model.Id);
            if (userInDb == null) return NotFound();

            var myPerms = GetMyPermissions();

            // Walidacja hierarchii
            if (!CanManage(myPerms, userInDb.AdminPermissions))
            {
                TempData["Error"] = "You do not have permission to modify this account.";
                return RedirectToAction(nameof(Users));
            }

            // Blokada nadawania rangi HeadAdmin (2)
            if (model.AdminPermissions == 2 && myPerms < 2)
            {
                model.AdminPermissions = userInDb.AdminPermissions; // Cofnij zmianę jeśli nie jesteś HeadAdminem
            }

            userInDb.FirstName = model.FirstName;
            userInDb.LastName = model.LastName ?? "";
            userInDb.Email = model.Email;
            userInDb.AdminPermissions = model.AdminPermissions;

            _context.Update(userInDb);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Data updated.";
            return RedirectToAction(nameof(Users));
        }

        // USUWANIE UŻYTKOWNIKA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            if (!CanManage(GetMyPermissions(), user.AdminPermissions) || id == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                TempData["Error"] = "You cannot delete this account.";
                return RedirectToAction(nameof(Users));
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "User deleted.";
            return RedirectToAction(nameof(Users));
        }

        // DODAWANIE UŻYTKOWNIKA
        [HttpGet]
        public IActionResult CreateUser() => View(new User());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(User user)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == user.Email || u.Username == user.Username))
                {
                    ModelState.AddModelError("", "The user already exists.");
                    return View(user);
                }

                // Blokada tworzenia HeadAdmina
                if (user.AdminPermissions == 2 && GetMyPermissions() < 2)
                {
                    user.AdminPermissions = 1;
                }

                user.CreatedDate = DateTime.Now;
                user.LastName = user.LastName ?? "";

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Account created.";
                return RedirectToAction(nameof(Users));
            }
            return View(user);
        }

        // BLOKOWANIE
        [HttpGet]
        public async Task<IActionResult> LockUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || !CanManage(GetMyPermissions(), user.AdminPermissions))
                return RedirectToAction(nameof(Users));

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUser(int id, int days, string reason)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || !CanManage(GetMyPermissions(), user.AdminPermissions)) return NotFound();

            user.LockoutEnd = DateTime.Now.AddDays(days);
            user.LockoutReason = reason;

            _context.Update(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Account blocked.";
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnlockUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || !CanManage(GetMyPermissions(), user.AdminPermissions)) return NotFound();

            user.LockoutEnd = null;
            user.LockoutReason = null;

            _context.Update(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Account unblocked.";
            return RedirectToAction(nameof(Users));
        }

        // PODSZYWANIE SIĘ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Impersonate(int id)
        {
            if (User.HasClaim("IsImpersonating", "true"))
            {
                TempData["Error"] = "You must first return to your account to log in as a different user.";
                return RedirectToAction(nameof(Users));
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null || !CanManage(GetMyPermissions(), user.AdminPermissions))
            {
                TempData["Error"] = "You cannot impersonate this user.";
                return RedirectToAction(nameof(Users));
            }

            var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            HttpContext.Session.SetString("OriginalAdminId", adminId);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("AdminPermissions", user.AdminPermissions.ToString()),
                new Claim(ClaimTypes.Role, user.AdminPermissions >= 1 ? "Admin" : "User"),
                new Claim("IsImpersonating", "true")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> RevertImpersonation()
        {
            var originalAdminId = HttpContext.Session.GetString("OriginalAdminId");
            if (string.IsNullOrEmpty(originalAdminId)) return RedirectToAction("Logout", "Auth");

            var adminUser = await _context.Users.FindAsync(int.Parse(originalAdminId));
            if (adminUser == null) return RedirectToAction("Logout", "Auth");

            HttpContext.Session.Remove("OriginalAdminId");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, adminUser.Id.ToString()),
                new Claim(ClaimTypes.Name, adminUser.Username),
                new Claim(ClaimTypes.Email, adminUser.Email),
                new Claim("AdminPermissions", adminUser.AdminPermissions.ToString()),
                new Claim(ClaimTypes.Role, adminUser.AdminPermissions == 2 ? "HeadAdmin" : "Admin")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return RedirectToAction("Users", "Admin");
        }
    }
}