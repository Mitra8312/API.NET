using System;
using System.Collections.Generic;

namespace APISolovki.Models;

public partial class Article
{
    public int? IdArticle { get; set; }

    public string? TegsArticle { get; set; } = null!;

    public string? ShortNameArticle { get; set; } = null!;

    public string? TextArticle { get; set; } = null!;

    public DateTime? DateOfPublishing { get; set; }

    public TimeSpan? TimeOfPublishing { get; set; }

    public int? ArticlePublisherId { get; set; }

}
