
﻿using ChickenFilmV2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ChickenFilmV2.Contacts;
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
            builder.Services.AddScoped<IShowtimesServices, ShowtimesServices>();
            builder.Services.AddScoped<ISeatsServices, SeatsServices>();

            // Add authentication using Cookie
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login"; // ???ng d?n ??n trang ??ng nh?p
                    options.LogoutPath = "/Account/Logout"; // ???ng d?n ??n trang ??ng xu?t
                    options.AccessDeniedPath = "/Account/AccessDenied"; // ???ng d?n ??n trang kh�ng c� quy?n
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Th?i gian h?t h?n c?a cookie
                    options.SlidingExpiration = true; // H?i sinh cookie n?u ng??i d�ng t??ng t�c
                });
            builder.Services.AddScoped<AdminDashboardViewModel>();           
            builder.Services.AddScoped<IShowtimeService, ShowtimeService>();

            // ??ng k� DbContext cho MovieDbContext
            builder.Services.AddDbContext<MovieDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Đăng ký MVC Controllers và Views
            builder.Services.AddControllersWithViews();

            // C?u h�nh Authorization (B?t t�nh n?ng x�c th?c)
            builder.Services.AddAuthorization(options =>
            {
                // ??nh ngh?a policy n?u c?n (V� d?: ch? cho ph�p Admin truy c?p)
                // options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            builder.Services.AddScoped<IBlogService, BlogService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // C?u h�nh Routing tr??c Authentication v� Authorization
            app.UseRouting();

            // D�ng Authentication v� Authorization
            app.UseAuthentication(); // D�ng Authentication ?? nh?n di?n ng??i d�ng
            app.UseAuthorization();   // D�ng Authorization ?? ki?m tra quy?n truy c?p

            // Default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
