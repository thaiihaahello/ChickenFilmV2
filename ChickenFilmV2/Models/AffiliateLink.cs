using System;
using System.Collections.Generic;

namespace ChickenFilmV2.Models;

public partial class AffiliateLink
{
    public int AffiliateLinkId { get; set; }

    public int UserId { get; set; }

    public string LinkCode { get; set; } = null!;

    public string TargetUrl { get; set; } = null!;

    public int? ClickCount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
