using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Rover.Domain;

namespace Rover.Infrastructure
{
    public class DataContext : IdentityDbContext // Updated to inherit from IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<userData> userData { get; set; }
        public DbSet<userAddress> userAddress { get; set; }
        public DbSet<bikeData> bikeData { get; set; }
        public DbSet<modelBike> modelBike { get; set; }
        public DbSet<orderUser> orderUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Important for IdentityDbContext

            modelBuilder.Entity<userData>().HasKey(u => u.Id);
            modelBuilder.Entity<userAddress>().HasKey(ua => ua.Id);
            modelBuilder.Entity<bikeData>().HasKey(b => b.IDbike);
            modelBuilder.Entity<modelBike>().HasKey(m => m.IDmodel);
            modelBuilder.Entity<orderUser>().HasKey(o => o.IDorder);

            modelBuilder.Entity<userAddress>()
                .HasOne<userData>()
                .WithOne()
                .HasForeignKey<userAddress>(ua => ua.Id);

            modelBuilder.Entity<bikeData>()
                .HasOne<modelBike>()
                .WithMany()
                .HasForeignKey(b => b.IDmodel);

            modelBuilder.Entity<orderUser>()
                .HasOne<userData>()
                .WithMany()
                .HasForeignKey(o => o.Id);

            modelBuilder.Entity<orderUser>()
                .HasOne<bikeData>()
                .WithMany()
                .HasForeignKey(o => o.IDbike);
        }
    }
}
