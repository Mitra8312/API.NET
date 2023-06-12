using System;
using System.Collections.Generic;

namespace APISolovki.Models;

public partial class Receipt
{
    public int? IdReceipt { get; set; }

    public decimal? FinalPrice { get; set; }

    public DateTime? DateOfReceipt { get; set; }

    public TimeSpan? TimeOfReceipt { get; set; }

    public int? PrisonerId { get; set; }

    public int? EmployeeId { get; set; }

}
