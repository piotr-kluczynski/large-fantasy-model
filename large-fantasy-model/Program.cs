using large_fantasy_model.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using large_fantasy_model.Models.CharacterModels.Json;
using large_fantasy_model.Hubs; 

namespace large_fantasy_model
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Rejestrowanie repozytoriów dla plików JSON
            builder.Services.AddScoped(sp =>
                new JsonRepository<Spell>("Data/JsonFiles"));
            builder.Services.AddScoped(f =>
                new JsonRepository<Feature>("Data/JsonFiles"));
            builder.Services.AddScoped(i =>
                new JsonRepository<Item>("Data/JsonFiles"));
            builder.Services.AddScoped(i =>
                new JsonRepository<CClass>("Data/JsonFiles"));
            builder.Services.AddScoped(i =>
                new JsonRepository<Race>("Data/JsonFiles"));
            builder.Services.AddScoped(i =>
                new JsonRepository<Weapon>("Data/JsonFiles"));
            builder.Services.AddScoped(i =>
                new JsonRepository<Background>("Data/JsonFiles"));

            builder.Services.AddControllersWithViews();

            builder.Services.AddHostedService<large_fantasy_model.Services.AvatarWatcherService>();

            builder.Services.AddSignalR();


            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); 
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


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

            app.UseSession();


            app.UseAuthentication();
            app.UseAuthorization(); 

     
            app.MapHub<PrivateMessageHub>("/privateMessageHub");
            app.MapHub<large_fantasy_model.Hubs.GameHub>("/gameHub");
            app.MapHub<large_fantasy_model.Hubs.LobbyHub>("/lobbyHub");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}