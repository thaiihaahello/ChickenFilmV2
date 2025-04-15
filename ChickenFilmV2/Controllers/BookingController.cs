using Microsoft.AspNetCore.Mvc;

namespace ChickenFilmV2.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Payment()
        {
            return View();
        }
    }
}
