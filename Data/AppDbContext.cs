using Microsoft.EntityFrameworkCore;
using WebApiCaching.Models;

namespace WebApiCaching.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeFutebol>()
                        .HasMany(t => t.Jogadores)  
                        .WithOne(j => j.TimeFutebol)  
                        .HasForeignKey(j => j.TimeFutebolId);

            modelBuilder.Entity<Jogador>()
                        .HasKey(t => t.Id);
            
            modelBuilder.Entity<Driver>()
                        .HasKey(t => t.Id);
        }


        public DbSet<Jogador> Jogadores { get; set;}
        public DbSet<TimeFutebol> TimeFutebols { get; set;}
        public DbSet<Driver> Drivers { get; set;}
    }
}

