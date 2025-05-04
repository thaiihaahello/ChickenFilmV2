using System;
using System.Collections.Generic;

namespace ChickenFilmV2.Models;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string Code { get; set; } = null!;

    public decimal Discount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool? IsActive { get; set; }

    public decimal? MinOrderValue { get; set; }

    public int MaxUsage { get; set; }

    public int? UsedCount { get; set; }
}
