using ChickenFilmV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChickenFilmV2.Controllers
{
    public class TicketController : Controller
    {
        private readonly MovieDbContext movieDbContext;

        public TicketController(MovieDbContext movieDbContext)
        {
            this.movieDbContext = movieDbContext;
        }

        // Hiển thị danh sách TicketPricing
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ticketPricings = await movieDbContext.TicketPricings
                .Include(tp => tp.Auditorium) // Load thêm thông tin phòng chiếu
                .ToListAsync();

            return View(ticketPricings);
        }


        // Hiển thị form Add
        [HttpGet]
        public IActionResult Add()
        {
            var auditoriums = movieDbContext.Auditoriums.ToList();
            ViewBag.Auditoriums = auditoriums;
            return View();
        }


        // Xử lý thêm mới TicketPricing
        [HttpPost]
        public async Task<IActionResult> Add(TicketPricing addTicketPricing)
        {
            // Kiểm tra xem auditorium đã có loại ghế này chưa
            bool ticketExists = await movieDbContext.TicketPricings.AnyAsync(
                tp => tp.AuditoriumId == addTicketPricing.AuditoriumId && tp.SeatType == addTicketPricing.SeatType);

            if (ticketExists)
            {
                ModelState.AddModelError("SeatType", "Phòng chiếu này đã có loại vé này.");

                // Lấy danh sách auditorium để hiển thị lại trên form
                ViewBag.Auditoriums = await movieDbContext.Auditoriums.ToListAsync();
                return View(addTicketPricing);
            }

            // Nếu chưa có, thêm vé mới vào cơ sở dữ liệu
            await movieDbContext.TicketPricings.AddAsync(addTicketPricing);
            await movieDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        // Hiển thị form Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var ticketPricing = await movieDbContext.TicketPricings.FindAsync(id);
            if (ticketPricing == null)
            {
                return NotFound();
            }

            ViewBag.Auditoriums = await movieDbContext.Auditoriums.ToListAsync();
            return View(ticketPricing);
        }

        [HttpPost]
        public async Task<IActionResult> Update(TicketPricing ticketPricing)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem có tồn tại bản ghi nào với cùng auditorium_id và seat_type nhưng có PricingId khác hay không
                bool duplicateExists = await movieDbContext.TicketPricings.AnyAsync(tp =>
                    tp.AuditoriumId == ticketPricing.AuditoriumId &&
                    tp.SeatType == ticketPricing.SeatType &&
                    tp.PricingId != ticketPricing.PricingId);

                if (duplicateExists)
                {
                    // Báo lỗi nếu đã tồn tại cặp dữ liệu này
                    ModelState.AddModelError("SeatType", "Phòng chiếu đã có giá vé cho loại ghế này.");

                    // Cần gán lại ViewBag.Auditoriums để hiển thị lại danh sách phòng chiếu
                    ViewBag.Auditoriums = await movieDbContext.Auditoriums.ToListAsync();
                    return View(ticketPricing);
                }

                // Nếu không có trùng lặp, tiến hành cập nhật
                movieDbContext.TicketPricings.Update(ticketPricing);
                await movieDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            // Nếu ModelState không hợp lệ thì cần gán lại ViewBag.Auditoriums
            ViewBag.Auditoriums = await movieDbContext.Auditoriums.ToListAsync();
            return View(ticketPricing);
        }


        // Xóa TicketPricing
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var ticketPricing = await movieDbContext.TicketPricings.FindAsync(id);
            if (ticketPricing != null)
            {
                movieDbContext.TicketPricings.Remove(ticketPricing);
                await movieDbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
