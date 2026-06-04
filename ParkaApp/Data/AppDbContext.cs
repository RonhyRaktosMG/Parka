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
        public DbSet<Occupation> Occupations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        


        // Configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Area & Place
            modelBuilder.Entity<Area>()
                .HasMany(a => a.Places)
                .WithOne(p => p.Area)
                .HasForeignKey(p => p.AreaId)
                .OnDelete(DeleteBehavior.Cascade);

            
            // Client & Payment
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Payments)
                .WithOne(p => p.Client)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // Place & Occupation
            modelBuilder.Entity<Occupation>()
                .HasOne(o => o.Place)
                .WithMany()
                .HasForeignKey(o => o.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Client & Occupation
            modelBuilder.Entity<Occupation>()
                .HasOne(o => o.Client)
                .WithMany()
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
                        
        }
    }
}