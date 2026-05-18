using ParkaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ListeEtudiant.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options): base(options)
        {}


        // DbSet
        public DbSet<Area> Areas { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Client> Clients { get; set; }


        // Configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Area & Place
            modelBuilder.Entity<Area>()
                .HasMany(a => a.Places)
                .WithOne()
                .HasForeignKey(p => p.AreaId)
                .OnDelete(DeleteBehavior.Cascade);

            
            // Client & Payment
            modelBuilder.Entity<Client>()
                .HasMany<Payment>()
                .WithOne(p => p.Client)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
            
        }
    }
}