using System;
using System.Collections.Generic;

namespace APISolovki.Models;

public partial class Product
{
    public int? IdProduct { get; set; }

    public string? NameProduct { get; set; } = null!;

    public decimal? PriceProduct { get; set; }

    public string? DescriptionProduct { get; set; } = null!;

    public int? CountProduct { get; set; }

    public int? TypeProductId { get; set; }

}
