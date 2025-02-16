using System;

namespace BlogAssignment.ViewModels;

public class ArticleViewModel
{
    public int ArticleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string ContributorId { get; set; } = string.Empty;
    public string ContributorFullName { get; set; } = string.Empty;
}
