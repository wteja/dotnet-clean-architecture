using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Contexts;

public class IdentityContext : IdentityDbContext<ApplicationUser>
{
    public IdentityContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Identity");
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable(name: "users");
        });
        builder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable(name: "roles");
        });
        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable(name: "users_roles");
        });
        builder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable(name: "users_claims");
        });
        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable(name: "users_logins");
        });
        builder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable(name: "roles_claims");
        });
        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable(name: "users_tokens");
        });
    }
}