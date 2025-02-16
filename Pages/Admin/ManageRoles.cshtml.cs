using BlogAssignment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogAssignment.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageRolesModel : PageModel
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageRolesModel(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<UserWithRoles> Users { get; set; } = new();

        [BindProperty]
        public string SelectedUserId { get; set; } = default!;
        
        [BindProperty]
        public string SelectedRole { get; set; } = default!;

        public class UserWithRoles
        {
            public string Id { get; set; } = default!;
            public string Email { get; set; } = default!;
            public List<string> Roles { get; set; } = new();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var users = _userManager.Users.ToList();
            Users = new List<UserWithRoles>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                Users.Add(new UserWithRoles
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Roles = roles.ToList()
                });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddRoleAsync()
        {
            var user = await _userManager.FindByIdAsync(SelectedUserId);
            if (user == null || string.IsNullOrEmpty(SelectedRole))
            {
                return Page();
            }

            if (!await _roleManager.RoleExistsAsync(SelectedRole))
            {
                ModelState.AddModelError("", "Role does not exist.");
                return Page();
            }

            await _userManager.AddToRoleAsync(user, SelectedRole);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveRoleAsync()
        {
            var user = await _userManager.FindByIdAsync(SelectedUserId);
            if (user == null || string.IsNullOrEmpty(SelectedRole))
            {
                return Page();
            }

            await _userManager.RemoveFromRoleAsync(user, SelectedRole);
            return RedirectToPage();
        }
    }
}
