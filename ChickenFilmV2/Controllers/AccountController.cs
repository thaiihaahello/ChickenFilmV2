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

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{

        //    // Kiểm tra xem email và mật khẩu có hợp lệ không (ko ma hoa)

        //    //if (!ModelState.IsValid) return View(model);

        //    //var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
        //    //if (user == null)
        //    //{
        //    //    ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
        //    //    return View(model);
        //    //}

        //    var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
        //    if (user == null)
        //    {
        //        ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
        //        return View(model);
        //    }

        //    var passwordHasher = new PasswordHasher<User>();
        //    var result = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

        //    if (result == PasswordVerificationResult.Failed)
        //    {
        //        ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
        //        return View(model);
        //    }


        //    var claims = new List<Claim>
        //{
        //    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        //    new Claim(ClaimTypes.Name, user.FullName ?? user.Username),
        //    new Claim(ClaimTypes.Role, user.Role ?? "User")
        //};

        //    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //    var principal = new ClaimsPrincipal(identity);

        //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        //    return RedirectToAction("Index", "Home");
        //}

        ////dang ki
        //// GET: Hiển thị trang đăng ký
        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View(); // Trả về view đăng ký cho người dùng
        //}

        //// POST: Xử lý khi người dùng gửi form đăng ký
        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Kiểm tra xem email đã tồn tại chưa
        //        var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
        //        if (existingUser != null)
        //        {
        //            ModelState.AddModelError("Email", "Email này đã được đăng ký.");
        //            return View(model);
        //        }

        //        // Kiểm tra xem username đã tồn tại chưa
        //        var existingUsername = _context.Users.FirstOrDefault(u => u.Username == model.Username);
        //        if (existingUsername != null)
        //        {
        //            ModelState.AddModelError("Username", "Username này đã tồn tại.");
        //            return View(model);
        //        }

        //        // Kiểm tra mật khẩu và xác nhận mật khẩu có khớp không
        //        if (model.Password != model.ConfirmPassword)
        //        {
        //            ModelState.AddModelError("ConfirmPassword", "Mật khẩu và xác nhận mật khẩu không khớp.");
        //            return View(model);
        //        }

        //        // Kiểm tra độ dài mật khẩu (min length: 6)
        //        if (model.Password.Length < 6)
        //        {
        //            ModelState.AddModelError("Password", "Mật khẩu phải có ít nhất 6 ký tự.");
        //            return View(model);
        //        }

        //        // Kiểm tra mật khẩu có chứa chữ hoa, số, và ký tự đặc biệt
        //        var passwordRegex = @"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])";
        //        if (!Regex.IsMatch(model.Password, passwordRegex))
        //        {
        //            ModelState.AddModelError("Password", "Mật khẩu phải chứa ít nhất một chữ hoa, một số và một ký tự đặc biệt.");
        //            return View(model);
        //        }

        //        // Kiểm tra số điện thoại có hợp lệ không (Regex ví dụ cho số điện thoại quốc tế)
        //        var phoneRegex = @"^0\d{9}$";
        //        if (!Regex.IsMatch(model.PhoneNumber, phoneRegex))
        //        {
        //            ModelState.AddModelError("PhoneNumber", "Số điện thoại không hợp lệ.");
        //            return View(model);
        //        }

        //        // Mã hóa mật khẩu
        //        var passwordHasher = new PasswordHasher<User>();
        //        var hashedPassword = passwordHasher.HashPassword(null, model.Password);

        //        var newUser = new User
        //        {
        //            FullName = model.FullName,
        //            Username = model.Username,
        //            Avatar = model.Avatar, // Set default avatar if none provided
        //            Gender = model.Gender,
        //            Birthday = model.Birthday,
        //            Email = model.Email,
        //            Password = hashedPassword,
        //            PhoneNumber = model.PhoneNumber,
        //            Address = model.Address,
        //            Role = "Customer",
        //            CreatedAt = DateTime.Now
        //        };

        //        _context.Users.Add(newUser);
        //        await _context.SaveChangesAsync();

        //        // Đăng nhập người dùng ngay lập tức sau khi đăng ký thành công
        //        var claims = new List<Claim>
        //{
        //    new Claim(ClaimTypes.NameIdentifier, newUser.UserId.ToString()),
        //    new Claim(ClaimTypes.Name, newUser.FullName),
        //    new Claim(ClaimTypes.Role, newUser.Role ?? "Customer")
        //};

        //        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //        var principal = new ClaimsPrincipal(identity);

        //        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        //        return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chủ
        //    }

        //    return View(model); // Trả về lại trang đăng ký nếu có lỗi
        //}

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
        new Claim(ClaimTypes.Name, user.FullName ?? user.Username),
        new Claim(ClaimTypes.Role, user.Role ?? "User")
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            //  PHÂN LUỒNG THEO ROLE
            switch (user.Role)
            {
                case "Admin":
                    return RedirectToAction("ForgotPassword", "Account"); // hoặc controller khác tuỳ bạn
                case "FilmManager":
                    return RedirectToAction("Index", "FilmManager");
                case "Customer":
                default:
                    return RedirectToAction("Payment", "Booking");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email này đã được đăng ký.");
                    return View(model);
                }

                var existingUsername = _context.Users.FirstOrDefault(u => u.Username == model.Username);
                if (existingUsername != null)
                {
                    ModelState.AddModelError("Username", "Username này đã tồn tại.");
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
                    Username = model.Username,
                    Avatar = model.Avatar,
                    Gender = model.Gender,
                    Birthday = model.Birthday,
                    Email = model.Email,
                    Password = model.Password, // ❗ Lưu trực tiếp mật khẩu không mã hóa
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
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
