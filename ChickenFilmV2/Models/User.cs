using System;
using System.Collections.Generic;

namespace ChickenFilmV2.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string Username { get; set; } = null!;

    public string Avatar { get; set; } = null!;

    public string? Role { get; set; }

    public bool Gender { get; set; }

    public DateOnly Birthday { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<AffiliateLink> AffiliateLinks { get; set; } = new List<AffiliateLink>();

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual ICollection<BookingAffiliate> BookingAffiliates { get; set; } = new List<BookingAffiliate>();

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<CommissionTransaction> CommissionTransactions { get; set; } = new List<CommissionTransaction>();
}
