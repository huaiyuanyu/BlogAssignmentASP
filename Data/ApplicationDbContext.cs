using BlogAssignment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BlogAssignment.Data;

public class ApplicationDbContext : IdentityDbContext<CustomUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        SeedUsersRoles seedData = new SeedUsersRoles();
        builder.Entity<IdentityRole>().HasData(seedData.Roles);
        builder.Entity<CustomUser>().HasData(seedData.Users);
        builder.Entity<IdentityUserRole<string>>().HasData(seedData.UserRoles);

        builder.Entity<Article>()
            .HasOne<CustomUser>()
            .WithMany()
            .HasForeignKey(a => a.ContributorId)
            .HasPrincipalKey(u => u.Id);

    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }
    }

public DbSet<BlogAssignment.Models.Article> Article { get; set; } = default!;

}
