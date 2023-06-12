using System;
using System.Collections.Generic;

namespace APISolovki.Models;

public partial class PrisonerInfo
{
    public string? ФиоЗаключенного { get; set; } = null!;

    public string? Статья { get; set; } = null!;

    public string? Работа { get; set; } = null!;

    public string? РодДеятельности { get; set; } = null!;

    public string? Каста { get; set; } = null!;

    public string? Нация { get; set; } = null!;

    public string? Здоровье { get; set; } = null!;

    public string? Логин { get; set; } = null!;
}
