using ChickenFilmV2.Contacts;
using ChickenFilmV2.Models;

namespace ChickenFilmV2.Services
{
    public class SeatsServices : ISeatsServices
    {
        private readonly MovieDbContext _context;

        public SeatsServices(MovieDbContext context)
        {
            _context = context;
        }

        public void GenerateSeats(Auditorium auditorium)
        {
            if (!int.TryParse(auditorium.ColumnNumber, out int columnCount))
                throw new ArgumentException("Invalid row or column number.");

            var rowLetters = auditorium.RowNumber.ToUpper();

            foreach (char row in rowLetters)
            {
                AddSeatsToDatabase(auditorium, row, columnCount);
                
            }
            _context.SaveChanges();
        }

        public void AddSeatsToDatabase(Auditorium auditorium, char row, int columnCount)
        {
            for (int col = 1; col <= columnCount; col++)
            {
                string seatNumber = $"{row}{col}";
                var seat = new Seat
                {
                    AuditoriumId = auditorium.AuditoriumId,
                    SeatNumber = seatNumber,
                    SeatType = "Standard",
                    IsAvailable = true
                };
                _context.Seats.Add(seat);
            }
        }

        public Auditorium? GetAuditorium (int id)
        {
            return _context.Auditoriums.FirstOrDefault(a => a.AuditoriumId == id);
        }
        
        public bool HasSeats(int id)
        {
            return _context.Seats.Any(s => s.AuditoriumId == id);
        }

        public void UpdateSeatType(List<Seat> seats)
        {
            foreach (var updated in seats)
            {
                var seat = _context.Seats.Find(updated.SeatId);
                if (seat != null)
                {
                    seat.SeatType = updated.SeatType;
                    seat.IsAvailable = updated.IsAvailable;
                    _context.Seats.Update(seat);
                }
            }
            _context.SaveChanges();
        }

        public List<Seat> GetSeatsByAuditoriumId(int auditoriumId)
        {
            return _context.Seats.Where(s => s.AuditoriumId == auditoriumId)
                .OrderBy(s => s.SeatNumber).ToList();
        }

        public Dictionary<string, List<Seat>> GetSeatsGroupedByRow(int id)
        {
            return _context.Seats
                .Where(s => s.AuditoriumId == id)
                .OrderBy(s => s.SeatNumber)
                .AsEnumerable()
                .GroupBy(s => s.SeatNumber[0].ToString()) 
                .ToDictionary(g => g.Key, g => g.ToList());
        }


    }
}