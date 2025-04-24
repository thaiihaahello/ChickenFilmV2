using ChickenFilmV2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

using ChickenFilmV2.Contacts;
using ChickenFilmV2.Models;
using ChickenFilmV2.Services;
using Microsoft.EntityFrameworkCore;
using ChickenFilmV2.Services.Interfaces.ChickenFilmV2.Services.Interfaces;
using ChickenFilmV2.ViewModels;

namespace ChickenFilmV2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<MovieDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IMoviesServices, MoviesServices>();
            builder.Services.AddScoped<IAuditoriumServices, AuditoriumServices>();

            // Add authentication using Cookie
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                });
            builder.Services.AddScoped<AdminDashboardViewModel>();           
            builder.Services.AddScoped<IShowtimeService, ShowtimeService>();

            // Đăng ký MVC Controllers và Views
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Use authentication and authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRouting();

            // Default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
