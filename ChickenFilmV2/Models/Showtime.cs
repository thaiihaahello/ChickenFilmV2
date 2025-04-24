using System;
using System.Collections.Generic;

namespace ChickenFilmV2.Models;

public partial class Showtime
{
    public int ShowtimeId { get; set; }

    public int MovieId { get; set; }

    public int AuditoriumId { get; set; }

    public DateTime? ShowDate { get; set; }

    public TimeOnly ShowTime1 { get; set; }

    public string? Status { get; set; }

    public virtual Auditorium Auditorium { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Movie Movie { get; set; } = null!;
}
