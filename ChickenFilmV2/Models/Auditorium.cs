using System;
using System.Collections.Generic;

namespace ChickenFilmV2.Models;

public partial class Auditorium
{
    public int AuditoriumId { get; set; }

    public int TheaterId { get; set; }

    public string AuditoriumName { get; set; } = null!;

    public string? AuditoriumType { get; set; }

    public string RowNumber { get; set; } = null!;

    public string ColumnNumber { get; set; } = null!;

    public int TotalSeats { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<AuditoriumSeatPricing> AuditoriumSeatPricings { get; set; } = new List<AuditoriumSeatPricing>();

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();

    public virtual Theater Theater { get; set; } = null!;

    public virtual ICollection<TicketPricing> TicketPricings { get; set; } = new List<TicketPricing>();
}
