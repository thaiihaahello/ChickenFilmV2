using ChickenFilmV2.Contacts;
using ChickenFilmV2.Models;
using ChickenFilmV2.ViewModelManager;
using Microsoft.AspNetCore.Mvc;

namespace ChickenFilmV2.Controllers
{
    public class AuditoriumController : Controller
    {
        private readonly MovieDbContext _context;
        private readonly IAuditoriumServices _auditoriumServices;
        private readonly ISeatsServices _seatsService;

        public AuditoriumController(MovieDbContext context, IAuditoriumServices auditoriumServices, ISeatsServices seatsService)
        {
            _context = context;
            _auditoriumServices = auditoriumServices;
            _seatsService = seatsService;
        }
        [HttpGet]
        public IActionResult IndexAuditoriumTheater()
        {
            var auditoriums = _auditoriumServices.GetAllAuditoriums();
            return View(auditoriums);
        }

        [HttpGet]
        public IActionResult CreateAuditorium()
        {
            var viewmodel = new AuditoriumViewModel();
            return View(viewmodel);
        }

        [HttpPost]
        public IActionResult CreateAuditorium(AuditoriumViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                var auditorium = new Auditorium()
                {
                    TheaterId = viewmodel.TheaterId,
                    AuditoriumName = viewmodel.AuditoriumName,
                    AuditoriumType = viewmodel.AuditoriumType,
                    RowNumber = viewmodel.RowNumber,
                    ColumnNumber = viewmodel.ColumnNumber,
                    TotalSeats = viewmodel.TotalSeats
                };
                _auditoriumServices.AddAuditorium(auditorium);
                return RedirectToAction("IndexAuditoriumTheater", "Auditorium");
            }
            return View(viewmodel);
        }
        [HttpGet]
        public IActionResult EditAuditorium(int id)
        {
            var auditorium = _auditoriumServices.GetAuditoriumById(id);
            if (auditorium != null)
            {
                var viewmodel = new Auditorium()
                {
                    AuditoriumId = auditorium.AuditoriumId,
                    TheaterId = auditorium.TheaterId,
                    AuditoriumName = auditorium.AuditoriumName,
                    AuditoriumType = auditorium.AuditoriumType,
                    RowNumber = auditorium.RowNumber,
                    ColumnNumber = auditorium.ColumnNumber,
                    TotalSeats = auditorium.TotalSeats
                };
                return View("EditAuditorium", viewmodel);
            }
            return RedirectToAction("IndexAuditoriumTheater", "Auditorium");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAuditorium(Auditorium model)
        {
            var auditorium = _auditoriumServices.GetAuditoriumById(model.AuditoriumId);
            if (auditorium != null)
            {
                auditorium.AuditoriumName = model.AuditoriumName;
                auditorium.AuditoriumType = model.AuditoriumType;
                auditorium.RowNumber = model.RowNumber;
                auditorium.ColumnNumber = model.ColumnNumber;
                auditorium.TotalSeats = model.TotalSeats;
                auditorium.CreatedAt = DateTime.Now;
                _auditoriumServices.UpdateAuditorium(auditorium);
                return RedirectToAction("IndexAuditoriumTheater", "Auditorium");
            }
            return RedirectToAction("IndexAuditoriumTheater", "Auditorium");
        }

        [HttpPost]
        public IActionResult DeleteAuditorium(int id)
        {
            var auditorium = _auditoriumServices.GetAuditoriumById(id);
            if (auditorium != null)
            {
                _auditoriumServices.DeleteAuditorium(id);
                return RedirectToAction("IndexAuditoriumTheater", "Auditorium");
            }
            return RedirectToAction("IndexAuditoriumTheater", "Auditorium");
        }


        public IActionResult ManageSeats(int id)
        {
            var auditorium = _seatsService.GetAuditorium(id);
            if (auditorium == null)
            {
                return NotFound();
            }

            var seats = _seatsService.GetSeatsByAuditoriumId(id);
            ViewBag.AudditoriumName = auditorium.AuditoriumName;

            return View(seats);
        }
        [HttpGet]
        public IActionResult GetSeatsGrid(int id)
        {
            var seatRows = _seatsService.GetSeatsGroupedByRow(id);
            if (seatRows == null || seatRows.Count == 0)
            {
                return NotFound();
            }

            return PartialView("GetSeatsGrid", seatRows);
        }
    }
}