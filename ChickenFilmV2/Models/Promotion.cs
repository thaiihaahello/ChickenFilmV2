using System;
using System.Collections.Generic;

namespace ChickenFilmV2.Models;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public int TheaterId { get; set; }

    public string Code { get; set; } = null!;

    public decimal Discount { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool? IsActive { get; set; }

    public decimal? MinOrderValue { get; set; }

    public int MaxUsage { get; set; }

    public int? UsedCount { get; set; }

    public virtual Theater Theater { get; set; } = null!;
}
