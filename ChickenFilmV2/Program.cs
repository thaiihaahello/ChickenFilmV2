using ChickenFilmV2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace ChickenFilmV2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add authentication using Cookie
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login"; // 
                    options.LogoutPath = "/Account/Logout"; // 
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // het han cookie
                    options.SlidingExpiration = true; // hoi sinh cookie
                });

            //  DbContext cho MovieDbContext
            builder.Services.AddDbContext<MovieDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //  Authorization 
            builder.Services.AddAuthorization(options =>
            {
                
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //  Authentication và Authorization
            app.UseRouting();

            // Dùng Authentication và Authorization
            app.UseAuthentication(); // Dùng Authentication 
            app.UseAuthorization();   // Dùng Authorization 

            // Default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
