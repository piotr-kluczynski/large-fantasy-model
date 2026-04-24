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
            // Przekazujemy pusty ViewModel do widoku
            return View(new ViewModels.RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Register(ViewModels.RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Sprawdzenie, czy email lub nazwa użytkownika są już zajęte
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email || u.Username == model.Username);

                if (existingUser != null)
                {
                    ViewBag.ErrorMessage = "Użytkownik z takim adresem e-mail lub nazwą już istnieje.";
                    return View(model);
                }

                // Przepisujemy dane z ViewModelu do obiektu bazy danych (User)
                var user = new User
                {
                    Username = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName ?? "",
                    Email = model.Email,
                    Password = model.Password, 
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