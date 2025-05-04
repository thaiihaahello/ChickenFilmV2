using ChickenFilmV2.Models;

namespace ChickenFilmV2.Contacts
{
    public interface ISeatsServices
    {
        public void GenerateSeats(Auditorium auditorium);
        public void AddSeatsToDatabase(Auditorium auditorium, char row, int columnCount);
        public void UpdateSeatType(List<Seat> seats);
        public Auditorium? GetAuditorium(int id);
        public bool HasSeats(int id);
        public List<Seat> GetSeatsByAuditoriumId(int auditoriumId);
        public Dictionary<string, List<Seat>> GetSeatsGroupedByRow(int id);
    }
}
