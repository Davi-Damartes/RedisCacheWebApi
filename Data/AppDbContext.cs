using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using WebApiCaching.Models;

namespace WebApiCaching.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("ConnectionString"));
        }

        public DbSet<Jogador> Jogadores { get; set;}
    }
}
