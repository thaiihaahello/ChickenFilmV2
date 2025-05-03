using Microsoft.AspNetCore.Mvc;
using ChickenFilmV2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace ChickenFilmV2.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly MovieDbContext movieDbContext;

        public ResetPasswordController(MovieDbContext movieDbContext)
        {
            this.movieDbContext = movieDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var user = await movieDbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Không tìm thấy người dùng với email này.");
                return View("Index", model);
            }

            if (user.Password != model.OldPassword)
            {
                ModelState.AddModelError("OldPassword", "Mật khẩu cũ không đúng.");
                return View("Index", model);
            }

            // Kiểm tra mật khẩu mới đủ mạnh
            var passwordRegex = @"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])";
            if (!Regex.IsMatch(model.NewPassword, passwordRegex))
            {
                ModelState.AddModelError("NewPassword", "Mật khẩu phải có ít nhất 1 chữ hoa, 1 số và 1 ký tự đặc biệt.");
                return View("Index", model);
            }

            user.Password = model.NewPassword;
            await movieDbContext.SaveChangesAsync();

            ViewBag.SuccessMessage = "Đổi mật khẩu thành công!";
            return View("Index");
        }


    }
}
