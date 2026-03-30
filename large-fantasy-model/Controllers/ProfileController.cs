using large_fantasy_model.Data;
using large_fantasy_model.Models;
using large_fantasy_model.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace large_fantasy_model.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly LargeFantasyModelContext _context;

        public ProfileController(LargeFantasyModelContext context)
        {
            _context = context;
        }

        // Zmienione z Index na ProfilePage
        public async Task<IActionResult> ProfilePage()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound();

            return View(user);
        }

        // --- EDYCJA PROFILU (GET) ---
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString!);

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            // ZAMIANA: Pakujemy dane z bazy do naszej "jednorazowej karteczki"
            var viewModel = new EditProfileViewModel
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Bio = user.Bio
            };

            return View(viewModel);
        }

        // --- EDYCJA PROFILU (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            // 1. Sprawdzamy, czy "karteczka" jest poprawnie wypełniona
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 2. Pobieramy użytkownika z bazy (ID bierzemy z bezpiecznego ciasteczka, a nie z formularza!)
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString!);

            var userInDb = await _context.Users.FindAsync(userId);
            if (userInDb == null) return NotFound();

            // Dodatkowe zabezpieczenie: jeśli zmienia Nick, upewnijmy się, że nikt inny go nie ma
            if (userInDb.Username != model.Username)
            {
                var usernameExists = await _context.Users.AnyAsync(u => u.Username == model.Username && u.Id != userId);
                if (usernameExists)
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                    return View(model);
                }
            }

            // 3. Przepisujemy dane z karteczki do akt w archiwum (do bazy)
            userInDb.Username = model.Username;
            userInDb.FirstName = model.FirstName;
            userInDb.LastName = model.LastName ?? "";
            userInDb.Bio = model.Bio ?? "";

            _context.Update(userInDb);
            await _context.SaveChangesAsync();

            // 4. Odświeżenie ciasteczka logowania (żeby nowy Nick od razu wskoczył do menu)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userInDb.Id.ToString()),
                new Claim(ClaimTypes.Name, userInDb.Username),
                new Claim(ClaimTypes.Email, userInDb.Email),
                new Claim("AdminPermissions", userInDb.AdminPermissions.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction(nameof(ProfilePage));
        }
        // --- ZMIANA HASŁA ---
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.Password != model.CurrentPassword)
            {
                ModelState.AddModelError("CurrentPassword", "Incorrect current password.");
                return View(model);
            }

            // Zmiana hasła
            user.Password = model.NewPassword;
            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ProfilePage)); // Po sukcesie wracamy na profil
        }

        // --- ZMIANA EMAILA ---
        [HttpGet]
        public IActionResult ChangeEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users.FindAsync(userId);

            if (user == null || user.Password != model.CurrentPassword)
            {
                ModelState.AddModelError("CurrentPassword", "Incorrect current password.");
                return View(model);
            }

            // Sprawdzamy, czy nowy email nie jest już zajęty
            var emailExists = await _context.Users.AnyAsync(u => u.Email == model.NewEmail && u.Id != userId);
            if (emailExists)
            {
                ModelState.AddModelError("NewEmail", "This email is already taken by another traveler.");
                return View(model);
            }

            // Zmiana emaila
            user.Email = model.NewEmail;
            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ProfilePage));
        }
    }
}