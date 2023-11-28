using System;
using System.Collections.Generic;

namespace BlueStarMVC.Models;

public partial class Chuyenbay
{
    public string FlyId { get; set; } = null!;

    public string? PlId { get; set; }

    public string? AirportId { get; set; }

    public string? FromLocation { get; set; }

    public string? ToLocation { get; set; }

    public DateTime? DepartureTime { get; set; }

    public DateTime? ArrivalTime { get; set; }

    public DateTime? DepartureDay { get; set; }

    public decimal? OriginalPrice { get; set; }
}
