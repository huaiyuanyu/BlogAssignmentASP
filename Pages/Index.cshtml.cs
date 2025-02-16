using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BlogAssignment.Data;
using BlogAssignment.Models;
using BlogAssignment.ViewModels;

namespace BlogAssignment.Pages_Articles
{
    public class IndexModel : PageModel
    {
        private readonly BlogAssignment.Data.ApplicationDbContext _context;

        public IndexModel(BlogAssignment.Data.ApplicationDbContext context)
        {
            _context = context;
        }

public IList<ArticleViewModel> Articles { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Articles = await _context.Article
            .OrderByDescending(a => a.EndDate)
            .Select(a => new ArticleViewModel
            {
                ArticleId = a.ArticleId,
                Title = a.Title,
                Body = a.Body,
                CreateDate = a.CreateDate,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                ContributorId = a.ContributorId,
                ContributorFullName = _context.Users
                    .Where(u => u.Id == a.ContributorId)
                    .Select(u => u.FirstName + " " + u.LastName)
                    .FirstOrDefault() ?? "Unknown"
            })
            .ToListAsync();
    }
    }
}
