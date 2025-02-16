using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogAssignment.Data;
using BlogAssignment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BlogAssignment.Pages_Articles
{
    [Authorize(Roles = "Admin,Contributor")]
    public class EditModel : PageModel
    {
        private readonly BlogAssignment.Data.ApplicationDbContext _context;
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;

        public EditModel(BlogAssignment.Data.ApplicationDbContext context, UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public Article Article { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article =  await _context.Article.FirstOrDefaultAsync(m => m.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }
            Article = article;

            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.Id != Article.ContributorId)
            {
                return Forbid();
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.Id != Article.ContributorId)
            {
              Console.WriteLine(user!.Id);
              Console.WriteLine(Article.ContributorId);
                return Forbid();
            }

      var existingArticle = await _context.Article.FindAsync(Article.ArticleId);
          if (existingArticle == null)
          {
              return NotFound();
          }

          if (!ModelState.IsValid)
          {
              return Page();
          }

          // Keep CreateDate and StartDate unchanged
          existingArticle.Title = Article.Title;
          existingArticle.Body = Article.Body;
          existingArticle.EndDate = DateTime.UtcNow;

          try
          {
              await _context.SaveChangesAsync();
          }
          catch (DbUpdateConcurrencyException)
          {
              if (!ArticleExists(Article.ArticleId))
              {
                  return NotFound();
              }
              else
              {
                  throw;
              }
          }

          return RedirectToPage("../Index");
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.ArticleId == id);
        }
    }
}
