using ChickenFilmV2.Models;

namespace ChickenFilmV2.Contacts
{
    public interface IAuditoriumServices
    {
        public List<Auditorium> GetAllAuditoriums();
        public Auditorium GetAuditoriumById(int id);
        public void AddAuditorium(Auditorium auditorium);
        public void UpdateAuditorium(Auditorium auditorium);
        public void DeleteAuditorium(int id);

        public List<string> GetAuditoriumNames();
    }
}
