using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BlogAssignment.Data;
using BlogAssignment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BlogAssignment.Pages_Articles
{
    [Authorize(Roles = "Admin,Contributor")]
    public class CreateModel : PageModel
    {
        private readonly BlogAssignment.Data.ApplicationDbContext _context;
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;

        public CreateModel(BlogAssignment.Data.ApplicationDbContext context, UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
          var user = await _userManager.GetUserAsync(User);
          if (user == null || !(await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Contributor")))
          {
              return Forbid();
          }

          Article = new Article
          {
              Title = "",
              Body = "",
              ContributorId = user.Id // Pre-populate ContributorId
          };

          return Page();
        }

        [BindProperty]
        public Article Article { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
          var user = await _userManager.GetUserAsync(User);
          if (user == null || !(await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "Contributor")))
          {
              return Forbid();
          }

          Article.CreateDate = DateTime.UtcNow;
          Article.StartDate = DateTime.UtcNow;
          Article.EndDate = DateTime.UtcNow;

            if (!ModelState.IsValid)
            {
              foreach (var error in ModelState.Values)
                {
                foreach (var e in error.Errors)
                {
                  Console.WriteLine(e.ErrorMessage);
                }
              }
              return Page();
            }

            _context.Article.Add(Article);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}
