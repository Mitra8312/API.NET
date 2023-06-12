using System;
using System.Collections.Generic;

namespace APISolovki.Models;

public partial class TypeProduct
{
    public int? IdTypeProduct { get; set; }

    public string? NameTypeProduct { get; set; } = null!;

    public int? DangerId { get; set; }

}
