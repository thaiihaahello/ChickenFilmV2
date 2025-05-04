using ChickenFilmV2.Contacts;
using ChickenFilmV2.Models;

namespace ChickenFilmV2.Services
{
    public class AuditoriumServices : IAuditoriumServices
    {
        private readonly MovieDbContext _context;

        public AuditoriumServices(MovieDbContext context)
        {
            _context = context;
        }

        public List<Auditorium> GetAllAuditoriums()
        {
            return _context.Auditoriums.ToList();
        }

        public Auditorium GetAuditoriumById(int id)
        {
            return _context.Auditoriums.Find(id);
        }

        public void AddAuditorium(Auditorium auditorium)
        {
            _context.Auditoriums.Add(auditorium);
            _context.SaveChanges();
        }

        public void UpdateAuditorium(Auditorium auditorium)
        {
            _context.Auditoriums.Update(auditorium);
            _context.SaveChanges();
        }

        public void DeleteAuditorium(int id)
        {
            var auditorium = GetAuditoriumById(id);
            if (auditorium != null)
            {
                _context.Auditoriums.Remove(auditorium);
                _context.SaveChanges();
            }
        }
    }
}
