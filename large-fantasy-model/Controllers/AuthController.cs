using large_fantasy_model.Data;
using large_fantasy_model.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace large_fantasy_model.Controllers
{
    public class AuthController : Controller
    {
        private readonly LargeFantasyModelContext _context;

        public AuthController(LargeFantasyModelContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View(new ViewModels.RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Register(ViewModels.RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email || u.Username == model.Username);

                if (existingUser != null)
                {
                    ViewBag.ErrorMessage = "A user with this email or username already exists.";
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName ?? "",
                    Email = model.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password), 
                    Bio = "",
                    CreatedDate = DateTime.Now,
                    AdminPermissions = 0
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Auth");
            }

           
            return View(model);
        }




        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                bool isPasswordValid = false;

                try 
                {
                    isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
                }
                catch (BCrypt.Net.SaltParseException)
                {
                }

                if (isPasswordValid)
                {
                if (user.IsLockedOut)
                {
                    ViewBag.ErrorMessage = $"Your access has been blocked until {user.LockoutEnd:dd.MM.yyyy}. Reason: {user.LockoutReason}";
                    return View();
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
                    new Claim(ClaimTypes.Name, user.Username),               
                    new Claim(ClaimTypes.Email, user.Email),                
                    new Claim("AdminPermissions", user.AdminPermissions.ToString())
                };

                if (!string.IsNullOrEmpty(user.ProfilePicturePath))
                {
                    claims.Add(new Claim("ProfilePicturePath", user.ProfilePicturePath));
                }

                if (user.AdminPermissions == 2) claims.Add(new Claim(ClaimTypes.Role, "HeadAdmin"));
                if (user.AdminPermissions >= 1) claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                if (user.AdminPermissions == 0) claims.Add(new Claim(ClaimTypes.Role, "User"));

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.ErrorMessage = "Invalid email address or password.";
            return View();
        }

        
        

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}