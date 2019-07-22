using Application.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application.API.Data {
    public class DataContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>> {
        public DataContext (DbContextOptions<DataContext> options) : base (options) { }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Voter> Voters { get; set; }
        public DbSet<VoterType> VoterTypes { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<VotingYears> VotingYears { get; set; }
        protected override void OnModelCreating (ModelBuilder builder) {
            base.OnModelCreating (builder);
            builder.Entity<VotingYears> ().HasKey (v => new { v.VoterId, v.OrganizationId, v.Year });
            builder.Entity<VotingYears> ().HasOne (v => v.Organization).WithMany (v => v.VotingYears).HasForeignKey (v => v.OrganizationId).OnDelete (DeleteBehavior.Restrict);
            builder.Entity<VotingYears> ().HasOne (v => v.Voter).WithMany (v => v.VotingYears).HasForeignKey (v => v.VoterId).OnDelete (DeleteBehavior.Restrict);
            builder.Entity<User> ().HasOne (u => u.Organization).WithMany (o => o.Users).OnDelete (DeleteBehavior.Cascade);
            builder.Entity<Voter> ().HasOne (v => v.VoterType).WithMany (v => v.Voters).OnDelete (DeleteBehavior.Cascade);
            builder.Entity<UserRole> (userRole => {

                userRole.HasKey (ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne (ur => ur.Role)
                    .WithMany (r => r.UserRoles)
                    .HasForeignKey (ur => ur.RoleId)
                    .IsRequired ();

                userRole.HasOne (ur => ur.User)
                    .WithMany (r => r.UserRoles)
                    .HasForeignKey (ur => ur.UserId)
                    .IsRequired ();
            });
        }
    }
}