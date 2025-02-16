using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BlogAssignment.Data;
using BlogAssignment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BlogAssignment.Pages_Articles
{
    [Authorize(Roles = "Admin,Contributor")]
    public class DeleteModel : PageModel
    {
        private readonly BlogAssignment.Data.ApplicationDbContext _context;
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;

        public DeleteModel(BlogAssignment.Data.ApplicationDbContext context, UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager)
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

            var article = await _context.Article.FirstOrDefaultAsync(m => m.ArticleId == id);

            if (article is not null)
            {
                Article = article;

                var user = await _userManager.GetUserAsync(User);
                if (user == null || user.Id != Article.ContributorId)
                {
                    return Forbid();
                }

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Article.FindAsync(id);
            if (article != null)
            {
                Article = article;

                var user = await _userManager.GetUserAsync(User);
                if (user == null || user.Id != Article.ContributorId)
                {
                    return Forbid();
                }

                _context.Article.Remove(Article);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Index");
        }
    }
}
