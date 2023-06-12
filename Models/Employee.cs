using System;
using System.Collections.Generic;

namespace APISolovki.Models;

public partial class Employee
{
    public int? IdEmployee { get; set; }

    public string? FirstNameEmployee { get; set; } = null!;

    public string? MiddleNameEmployee { get; set; }

    public string? SecondNameEmployee { get; set; } = null!;

    public string? LoginEmployee { get; set; } = null!;

    public string? PasswordEmployee { get; set; } = null!;

    public int? PostId { get; set; }

    public string? SaltEmployee { get; set; }

}
