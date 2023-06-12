using System;
using System.Collections.Generic;

namespace APISolovki.Models;

public partial class IndividualOffer
{
    public int? IdIndividualOffers { get; set; }

    public string? NameIndividualOffers { get; set; } = null!;

    public string? DescriptionIndividualOffer { get; set; } = null!;

}
