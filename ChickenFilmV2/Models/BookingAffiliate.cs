using System;
using System.Collections.Generic;

namespace ChickenFilmV2.Models;

public partial class BookingAffiliate
{
    public int BookingAffiliateId { get; set; }

    public int BookingId { get; set; }

    public int? UserId { get; set; }

    public decimal CommissionAmount { get; set; }

    public string? TransactionStatus { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual User? User { get; set; }
}
