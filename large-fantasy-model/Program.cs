using large_fantasy_model.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using large_fantasy_model.Models.CharacterModels.Json;

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

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<LargeFantasyModelContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("LargeFantasyModelDB"))
            );

            // Konfiguracja autoryzacji opartej na ciasteczkach
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login"; // Tu przekieruje, jak ktoś wejdzie tam, gdzie nie ma dostępu
                });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
