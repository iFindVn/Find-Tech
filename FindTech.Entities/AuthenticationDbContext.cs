using FindTech.Entities.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FindTech.Entities
{
    public class AuthenticationDbContext : IdentityDbContext<FindTechUser>
    {
        public AuthenticationDbContext()
            : base("FindTechContext", throwIfV1Schema: false)
        {
        }

        public static AuthenticationDbContext Create()
        {
            return new AuthenticationDbContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>()
                .ToTable("FindTechUsers", "dbo");
            modelBuilder.Entity<FindTechUser>()
                .ToTable("FindTechUsers", "dbo");
            modelBuilder.Entity<IdentityRole>()
                .ToTable("FindTechRoles", "dbo");
            modelBuilder.Entity<IdentityUserRole>()
                .ToTable("FindTechUserRoles", "dbo");
            modelBuilder.Entity<IdentityUserClaim>()
                .ToTable("FindTechUserClaims", "dbo");
            modelBuilder.Entity<IdentityUserLogin>()
                .ToTable("FindTechUserLogins", "dbo");
        }

    }
}