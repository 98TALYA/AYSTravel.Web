using AYSTravel.Data.Entities;
using AYSTravel.Logic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AYSTravel.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ville> Villes { get; set; }
        public DbSet<Activite> Activites { get; set; }
        public DbSet<Match> Matchs { get; set; }
        public DbSet<Monument> Monuments { get; set; }
        public DbSet<MoyenTransport> MoyensTransports { get; set; }
        public DbSet<Restauration> Restaurations { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Stade> Stades { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔹 Stade -> Ville
            modelBuilder.Entity<Stade>()
                .HasOne(s => s.Ville)
                .WithMany(v => v.Stades)
                .HasForeignKey(s => s.VilleId)
                .OnDelete(DeleteBehavior.Restrict); // NO ACTION

            // 🔹 Match -> Stade
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Stade)
                .WithMany(s => s.Matchs)
                .HasForeignKey(m => m.StadeId)
                .OnDelete(DeleteBehavior.Restrict); // NO ACTION
        }
    }
}