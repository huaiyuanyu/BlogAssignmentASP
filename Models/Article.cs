using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAssignment.Models;

public class Article
{
    [Key]
    public int ArticleId { get; set; }

    [Required]
    [StringLength(200)] // Adjust as needed
    public required string Title { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    public required string Body { get; set; }

    [Required]
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    [Required]
    public required string ContributorId { get; set; }
}
