using AYSTravel.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
    }
}
