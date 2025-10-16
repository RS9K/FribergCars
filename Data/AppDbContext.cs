using Microsoft.EntityFrameworkCore;
using FribergCars.Models;   

namespace FribergCars.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Car> Cars { get; set; } 
        public DbSet<Customer> Customers { get; set; } 
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
