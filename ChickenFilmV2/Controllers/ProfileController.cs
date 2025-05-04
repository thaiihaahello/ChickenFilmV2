using ChickenFilmV2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChickenFilmV2.Controllers
{
    public class ProfileController : Controller
    {
        private readonly MovieDbContext movieDbContext;

        public ProfileController(MovieDbContext movieDbContext)
        {
            this.movieDbContext = movieDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> View()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdClaim.Value);
            var user = movieDbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
                return NotFound();

            return View(user);
        }

        
        [HttpGet]
        public IActionResult EditProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdClaim.Value);
            var user = movieDbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return NotFound();

            return View(user);
        }

        
        [HttpPost]
        public async Task<IActionResult> EditProfile(User model, string? newPassword)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return RedirectToAction("Login", "Account");

            int userId = int.Parse(userIdClaim.Value);
            var user = movieDbContext.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return NotFound();

            // Cập nhật thông tin
            user.FullName = model.FullName;
            user.Gender = model.Gender;
            user.Birthday = model.Birthday;
            user.PhoneNumber = model.PhoneNumber;
            user.Avatar = model.Avatar;

            await movieDbContext.SaveChangesAsync();

            // Làm mới cookie xác thực để cập nhật lại tên trong User.Identity.Name
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName), // Cập nhật tên mới
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            // Xóa cookie cũ và đăng nhập lại
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignInAsync("Cookies", principal);

            TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("View");
        }
    }
}
