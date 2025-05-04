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
                    options.LoginPath = "/Account/Login"; // ???ng d?n ??n trang ??ng nh?p
                    options.LogoutPath = "/Account/Logout"; // ???ng d?n ??n trang ??ng xu?t
                    options.AccessDeniedPath = "/Account/AccessDenied"; // ???ng d?n ??n trang không có quy?n
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Th?i gian h?t h?n c?a cookie
                    options.SlidingExpiration = true; // H?i sinh cookie n?u ng??i dùng t??ng tác
                });

            // ??ng ký DbContext cho MovieDbContext
            builder.Services.AddDbContext<MovieDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // C?u hình Authorization (B?t tính n?ng xác th?c)
            builder.Services.AddAuthorization(options =>
            {
                // ??nh ngh?a policy n?u c?n (Ví d?: ch? cho phép Admin truy c?p)
                // options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
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

            // C?u hình Routing tr??c Authentication và Authorization
            app.UseRouting();

            // Dùng Authentication và Authorization
            app.UseAuthentication(); // Dùng Authentication ?? nh?n di?n ng??i dùng
            app.UseAuthorization();   // Dùng Authorization ?? ki?m tra quy?n truy c?p

            // Default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
