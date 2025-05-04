using ChickenFilmV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ChickenFilmV2.Controllers
{
    public class PromotionController : Controller
    {
        private readonly MovieDbContext movieDbContext;

        public PromotionController(MovieDbContext movieDbContext)
        {
            this.movieDbContext = movieDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var promotions = await movieDbContext.Promotions.ToListAsync();
            return View(promotions);
        }

        // Trang tạo promotion
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // Xử lý thêm promotion
        [HttpPost]
        public async Task<IActionResult> Add(Promotion addPromotionRequest)
        {
            
                var promotion = new Promotion
                {
                    Code = addPromotionRequest.Code,
                    Discount = addPromotionRequest.Discount,
                    StartDate = addPromotionRequest.StartDate,
                    EndDate = addPromotionRequest.EndDate,
                    IsActive = addPromotionRequest.IsActive,
                    MinOrderValue = addPromotionRequest.MinOrderValue,
                    MaxUsage = addPromotionRequest.MaxUsage,
                    UsedCount = 0
                };

                await movieDbContext.Promotions.AddAsync(promotion);
                await movieDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
        }

        // Trang cập nhật promotion (GET)
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var promotion = await movieDbContext.Promotions.FindAsync(id);

            if (promotion == null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        // Xử lý cập nhật promotion (POST)
        [HttpPost]
        public async Task<IActionResult> Update( Promotion updatePromotionRequest)
        {
            if (ModelState.IsValid)
            {
                var existingPromotion = await movieDbContext.Promotions.FindAsync(updatePromotionRequest.PromotionId);

                if (existingPromotion == null)
                {
                    return NotFound();
                }
                existingPromotion.Code = updatePromotionRequest.Code;
                existingPromotion.Discount = updatePromotionRequest.Discount;
                existingPromotion.StartDate = updatePromotionRequest.StartDate;
                existingPromotion.EndDate = updatePromotionRequest.EndDate;
                existingPromotion.IsActive = updatePromotionRequest.IsActive;
                existingPromotion.MinOrderValue = updatePromotionRequest.MinOrderValue;
                existingPromotion.MaxUsage = updatePromotionRequest.MaxUsage;
                // UsedCount không chỉnh sửa từ form

                await movieDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(updatePromotionRequest);
        }

        // Xử lý xoá promotion
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var promotion = await movieDbContext.Promotions.FindAsync(id);

            if (promotion == null)
            {
                return NotFound();
            }

            movieDbContext.Promotions.Remove(promotion);
            await movieDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
