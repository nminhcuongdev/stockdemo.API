using Microsoft.EntityFrameworkCore;
using StockDemo.API.Models.Domain;

namespace StockDemo.API.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Keep entity/table mapping explicit for the auth DB
            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}