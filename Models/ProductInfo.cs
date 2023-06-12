using System;
using System.Collections.Generic;

namespace APISolovki.Models;

public partial class ProductInfo
{
    public string? НазваниеТовара { get; set; } = null!;

    public string? ВидТовара { get; set; } = null!;

    public string? ОпасностьТовара { get; set; } = null!;

    public decimal? Стоимость { get; set; }

    public string? Описание { get; set; } = null!;
}
