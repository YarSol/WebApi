using Jax3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jax3.Persistence
{
    public class JaxDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Competition> Competitions { get; set; }

        public JaxDbContext(DbContextOptions<JaxDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(p => new { p.Email })
                .IsUnique()
                .HasName("UniqueError_User_Email");

            modelBuilder.Entity<Competition>()
                .HasOne(k => k.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CompetitionUser>()
                .HasKey(k => new
                {
                    k.CompetitionId,
                    k.UserId
                });

            modelBuilder.Entity<CompetitionUser>()
                .HasOne(k => k.Competition)
                .WithMany(k => k.Participants)
                .HasForeignKey("CompetitionId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
