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

            builder.Services.AddScoped<JsonRepository<Spell>>(sp =>
                new JsonRepository<Spell>("Data/JsonFiles"));

            builder.Services.AddControllersWithViews();


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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}