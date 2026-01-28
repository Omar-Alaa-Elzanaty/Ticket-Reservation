using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Adapter.SqlServer
{
    public class TicketDbContext(DbContextOptions<TicketDbContext> options) : IdentityDbContext<User>(options)
    {
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            AddIdentityTables(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(TicketDbContext).Assembly);
        }

        private static void AddIdentityTables(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("Accounts")
                            .Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        }
    }
}
