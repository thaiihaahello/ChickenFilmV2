using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ChickenFilmV2.Models;

public partial class TicketPricing
{
    public int PricingId { get; set; }

    public int AuditoriumId { get; set; }

    public string SeatType { get; set; } = null!;

    public decimal Price { get; set; }

    [ValidateNever] 
    public  Auditorium Auditorium { get; set; } = null!;
}
