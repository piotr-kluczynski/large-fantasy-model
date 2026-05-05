using large_fantasy_model.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using large_fantasy_model.Models.CharacterModels.Json;
using large_fantasy_model.Hubs; // --- SIGNALR: Dodany using, żeby widział Twój Hub ---

namespace large_fantasy_model
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Rejestrowanie repozytoriów dla plików JSON
            builder.Services.AddScoped<JsonRepository<Spell>>(sp =>
                new JsonRepository<Spell>("Data/JsonFiles"));

            builder.Services.AddControllersWithViews();

            // --- SIGNALR: Rejestracja serwisu w aplikacji ---
            builder.Services.AddSignalR();
            // ------------------------------------------------

            // --- DODAJ TO: Rejestracja obsługi Sesji ---
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Sesja wygaśnie po 30 min bezczynności
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            // ------------------------------------------

            builder.Services.AddDbContext<LargeFantasyModelContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("LargeFantasyModelDB"))
            );

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // --- DODAJ TO: Włączenie Sesji w potoku (MUSI BYĆ PRZED Authentication) ---
            app.UseSession();
            // -------------------------------------------------------------------------

            app.UseAuthentication();
            app.UseAuthorization(); // Zostawiamy tylko jedno

            // --- SIGNALR: Mapowanie ścieżki do Huba ---
            app.MapHub<PrivateMessageHub>("/privateMessageHub");
            // ------------------------------------------

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}