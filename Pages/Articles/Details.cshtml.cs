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
using Microsoft.AspNetCore.Identity;

namespace BlogAssignment.Pages_Articles
{
    public class DetailsModel : PageModel
    {
        private readonly BlogAssignment.Data.ApplicationDbContext _context;
        private readonly UserManager<CustomUser> _userManager;

        public DetailsModel(BlogAssignment.Data.ApplicationDbContext context, UserManager<CustomUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ArticleViewModel Article { get; set; } = default!;
        public CustomUser CurrentUser { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            CurrentUser = user!;

            var article = await _context.Article
            .Where(m => m.ArticleId == id)
            .Select(m => new ArticleViewModel
            {
                ArticleId = m.ArticleId,
                Title = m.Title,
                Body = m.Body,
                CreateDate = m.CreateDate,
                StartDate = m.StartDate,
                EndDate = m.EndDate,
                ContributorId = m.ContributorId,
                ContributorFullName = _context.Users
                    .Where(u => u.Id == m.ContributorId)
                    .Select(u => u.FirstName + " " + u.LastName)
                    .FirstOrDefault() ?? "Unknown"
            })
            .FirstOrDefaultAsync();

            if (article is not null)
            {
                Article = article;


                return Page();
            }

            return NotFound();
        }
    }
}
