namespace ChickenFilmV2.ViewModelManager
{
    public class AuditoriumViewModel
    {
        public int TheaterId { get; set; }
        public string AuditoriumName { get; set; } = null!;
        public string? AuditoriumType { get; set; }
        public string RowNumber { get; set; } = null!;
        public string ColumnNumber { get; set; } = null!;
        public int TotalSeats { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
