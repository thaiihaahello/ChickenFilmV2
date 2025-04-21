using System;
using System.Collections.Generic;

namespace ChickenFilmV2.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int BookingId { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string? PaymentStatus { get; set; }

    public string? TransactionId { get; set; }

    public DateTime? TransactionDate { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual Booking Booking { get; set; } = null!;
}
