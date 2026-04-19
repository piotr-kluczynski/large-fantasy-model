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

        // Konstruktor wstrzykujący bazę danych
        public AuthController(LargeFantasyModelContext context)
        {
            _context = context;
        }

        // REJESTRACJA

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Sprawdzenie, czy email lub nazwa użytkownika są już zajęte
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == user.Email || u.Username == user.Username);

                if (existingUser != null)
                {
                    ViewBag.ErrorMessage = "Użytkownik z takim adresem e-mail lub nazwą już istnieje.";
                    return View(user);
                }

                // Ustawienie domyślnych wartości wymaganych przez model User
                user.LastName = user.LastName ?? "";

                // Bio domyślnie zostawiamy puste, do edycji później w Profilu
                user.Bio = "";

                user.CreatedDate = DateTime.Now; 
                user.AdminPermissions = 0;       

                
                // Na ten moment zapisujemy tak, jak pozwala na to model.

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Auth");
            }

            // Jeśli dane z formularza były błędne, wracamy do widoku z błędami
            return View(user);
        }

        
        // LOGOWANIE
        

        [HttpGet]
        public IActionResult Login()
        {
            // Po zalogowaniu wrcamy do strony Home
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Szukamy użytkownika pasującego do maila i hasła
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // Tworzymy listę "Dowodów tożsamości" (Claims)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
                    new Claim(ClaimTypes.Name, user.Username),               
                    new Claim(ClaimTypes.Email, user.Email),                
                    new Claim("AdminPermissions", user.AdminPermissions.ToString()) 
                };

                // Pakujemy to w "tożsamość" opartą na ciasteczkach
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Fizyczne zalogowanie (zapisanie ciasteczka w przeglądarce)
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                // Po sukcesie wrzucamy gracza na stronę główną
                return RedirectToAction("Index", "Home");
            }

            // Jeśli podano złe dane
            ViewBag.ErrorMessage = "Nieprawidłowy adres e-mail lub hasło.";
            return View();
        }

        
        // WYLOGOWYWANIE
        

        public async Task<IActionResult> Logout()
        {
            // Usuwamy ciasteczko logowania
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Wracamy na stronę główną
            return RedirectToAction("Index", "Home");
        }
    }
}