using System;
using System.Collections.Generic;

namespace BlueStarMVC.Models;

public partial class Discount
{
    public string DId { get; set; } = null!;

    public string? DName { get; set; }

    public DateTime? DStart { get; set; }

    public DateTime? DFinish { get; set; }

    public int? DPercent { get; set; }
}
