namespace ChickenFilmV2.ViewModels
{
    public class SeatViewModel
    {
        public int SeatId { get; set; }
        public string SeatNumber { get; set; }
        public string? SeatType { get; set; }
        public bool IsAvailable { get; set; }

        public decimal Price { get; set; }

    }

}
