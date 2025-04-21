using System;
using System.Collections.Generic;

namespace ChickenFilmV2.Models;

public partial class CommissionTransaction
{
    public int CommissionTransactionId { get; set; }

    public int UserId { get; set; }

    public int? BookingId { get; set; }

    public int? TheaterId { get; set; }

    public decimal Amount { get; set; }

    public string? CommissionType { get; set; }

    public string? Status { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string? PaymentStatus { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual Theater? Theater { get; set; }

    public virtual User User { get; set; } = null!;
}
