using System;
using System.Collections.Generic;

namespace APISolovki.Models;

public partial class Prisoner
{
    public int? IdPrisoner { get; set; }

    public string? FirstNamePrisoner { get; set; } = null!;

    public string? MiddleNamePrisoner { get; set; }

    public string? SecondNamePrisoner { get; set; } = null!;

    public string? LoginPrisoner { get; set; } = null!;

    public string? PasswordPrisoner { get; set; } = null!;

    public int? NationId { get; set; }

    public int? HealthId { get; set; }

    public int? IndividualOffersId { get; set; }

    public int? TypeOfActivityId { get; set; }

    public int? WorkId { get; set; }

    public int? ArticleOfTheConclusionId { get; set; }

    public int? CasteId { get; set; }

    public string? Salt { get; set; }

}
