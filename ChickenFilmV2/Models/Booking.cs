using System;
using System.Collections.Generic;

namespace ChickenFilmV2.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? UserId { get; set; }

    public int ShowtimeId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int SeatId { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual ICollection<BookingAffiliate> BookingAffiliates { get; set; } = new List<BookingAffiliate>();

    public virtual ICollection<CommissionTransaction> CommissionTransactions { get; set; } = new List<CommissionTransaction>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Seat Seat { get; set; } = null!;

    public virtual ICollection<SeatBooking> SeatBookings { get; set; } = new List<SeatBooking>();

    public virtual Showtime Showtime { get; set; } = null!;

    public virtual User? User { get; set; }
}
