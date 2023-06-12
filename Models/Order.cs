using System;
using System.Collections.Generic;

namespace APISolovki.Models;

public partial class Order
{
    public int? IdOrder { get; set; }

    public int? RecieptId { get; set; }

    public int? ProductId { get; set; }

}
