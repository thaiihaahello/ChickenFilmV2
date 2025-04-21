using System;
using System.Collections.Generic;

namespace ChickenFilmV2.Models;

public partial class AuditLog
{
    public int LogId { get; set; }

    public int? UserId { get; set; }

    public string? ActionType { get; set; }

    public int? BookingId { get; set; }

    public int? PaymentId { get; set; }

    public DateTime? ActionTimestamp { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual User? User { get; set; }
}
