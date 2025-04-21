using System;
using System.Collections.Generic;

namespace ChickenFilmV2.Models;

public partial class Theater
{
    public int TheaterId { get; set; }

    public string TheaterName { get; set; } = null!;

    public string? MapUrl { get; set; }

    public string Location { get; set; } = null!;

    public int TotalTheaters { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Auditorium> Auditoria { get; set; } = new List<Auditorium>();

    public virtual ICollection<CommissionTransaction> CommissionTransactions { get; set; } = new List<CommissionTransaction>();

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}
