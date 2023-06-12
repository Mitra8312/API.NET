using System;
using System.Collections.Generic;

namespace APISolovki.Models;

public partial class FavoriteArticle
{
    public int? IdFavoriteArticle { get; set; }

    public int? ArticleId { get; set; }

    public int? PrisonerId { get; set; }

}
