using ChickenFilmV2.Models;
using ChickenFilmV2.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace ChickenFilmV2.Controllers
{
    public class AccountController : Controller
    {
        private readonly MovieDbContext _context;

        public AccountController(MovieDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
                return View(model);
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Name, user.FullName ?? user.FullName),
        new Claim(ClaimTypes.Role, user.Role ?? "User")
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            //  PHÂN LUỒNG THEO ROLE
            switch (user.Role)
            {
                case "Admin":
                    return RedirectToAction("Index", "Admin"); // hoặc controller khác tuỳ bạn
                case "FilmManager":
                    return RedirectToAction("Index", "FilmManager");
                case "Customer":
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            Console.WriteLine("Gọi vào POST Register");
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email này đã được đăng ký.");
                    return View(model);
                }

                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Mật khẩu và xác nhận mật khẩu không khớp.");
                    return View(model);
                }

                if (model.Password.Length < 8)
                {
                    ModelState.AddModelError("Password", "Mật khẩu phải có ít nhất 8 ký tự.");
                    return View(model);
                }

                var passwordRegex = @"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])";
                if (!Regex.IsMatch(model.Password, passwordRegex))
                {
                    ModelState.AddModelError("Password", "Mật khẩu phải chứa ít nhất một chữ hoa, một số và một ký tự đặc biệt.");
                    return View(model);
                }

                var phoneRegex = @"^0\d{9}$";
                if (!Regex.IsMatch(model.PhoneNumber, phoneRegex))
                {
                    ModelState.AddModelError("PhoneNumber", "Số điện thoại không hợp lệ.");
                    return View(model);
                }

                var newUser = new User
                {
                    FullName = model.FullName,
                    Avatar = model.Avatar,
                    Gender = model.Gender,
                    Birthday = model.Birthday,
                    Email = model.Email,
                    Password = model.Password, // ❗ Lưu trực tiếp mật khẩu không mã hóa
                    PhoneNumber = model.PhoneNumber,
                    Role = "Customer",
                    CreatedAt = DateTime.Now
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, newUser.UserId.ToString()),
            new Claim(ClaimTypes.Name, newUser.FullName),
            new Claim(ClaimTypes.Role, newUser.Role ?? "Customer")
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


        //quen pass
        public IActionResult ForgotPassword()
        {
            return View();
        }



    }


}
