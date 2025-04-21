using System;
using System.Collections.Generic;

namespace ChickenFilmV2.Models;

public partial class AuditoriumSeatPricing
{
    public int PricingId { get; set; }

    public int AuditoriumId { get; set; }

    public string SeatType { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual Auditorium Auditorium { get; set; } = null!;
}
