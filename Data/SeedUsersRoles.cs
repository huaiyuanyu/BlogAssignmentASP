using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using BlogAssignment.Models;  // Assuming CustomUser is in this namespace

namespace BlogAssignment.Data
{
    public class SeedUsersRoles
    {
        private readonly List<IdentityRole> _roles;
        private readonly List<CustomUser> _users;
        private readonly List<IdentityUserRole<string>> _userRoles;

        public SeedUsersRoles()
        {
            _roles = GetRoles();
            _users = GetUsers();
            _userRoles = GetUserRoles(_users, _roles);
        }

        public List<IdentityRole> Roles { get { return _roles; } }
        public List<CustomUser> Users { get { return _users; } }
        public List<IdentityUserRole<string>> UserRoles { get { return _userRoles; } }

        private List<IdentityRole> GetRoles()
        {
            // Seed Roles
            var adminRole = new IdentityRole("Admin");
            adminRole.NormalizedName = adminRole.Name!.ToUpper();

            var contributorRole = new IdentityRole("Contributor");
            contributorRole.NormalizedName = contributorRole.Name!.ToUpper();

            var memberRole = new IdentityRole("Member");
            memberRole.NormalizedName = memberRole.Name!.ToUpper();

            List<IdentityRole> roles = new List<IdentityRole>() {
                adminRole,
                contributorRole,
                memberRole
            };

            return roles;
        }

        private List<CustomUser> GetUsers()
        {
            string pwd = "P@$$w0rd";
            var passwordHasher = new PasswordHasher<CustomUser>();

            // Seed Users
            var adminUser = new CustomUser
            {
                UserName = "a@a.a",
                Email = "a@a.a",
                FirstName = "John",
                LastName = "Doe",
                EmailConfirmed = true,
            };
            adminUser.NormalizedUserName = adminUser.UserName.ToUpper();
            adminUser.NormalizedEmail = adminUser.Email.ToUpper();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, pwd);

            var contributorUser = new CustomUser
            {
                UserName = "c@c.c",
                Email = "c@c.c",
                FirstName = "Jane",
                LastName = "Doe",
                EmailConfirmed = true,
            };
            contributorUser.NormalizedUserName = contributorUser.UserName.ToUpper();
            contributorUser.NormalizedEmail = contributorUser.Email.ToUpper();
            contributorUser.PasswordHash = passwordHasher.HashPassword(contributorUser, pwd);

            List<CustomUser> users = new List<CustomUser>()
            {
                adminUser,
                contributorUser,
            };

            return users;
        }

        private List<IdentityUserRole<string>> GetUserRoles(List<CustomUser> users, List<IdentityRole> roles)
        {
            // Seed UserRoles
            List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();

            userRoles.Add(new IdentityUserRole<string>
            {
                UserId = users[0].Id, // Admin User
                RoleId = roles.First(q => q.Name == "Admin").Id
            });

            userRoles.Add(new IdentityUserRole<string>
            {
                UserId = users[1].Id, // Contributor User
                RoleId = roles.First(q => q.Name == "Contributor").Id
            });

            return userRoles;
        }
    }
}
