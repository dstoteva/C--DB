using CarDealer.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;

namespace CarDealer.Data
{
    public class CarDealerContext : DbContext
    {
        public CarDealerContext(DbContextOptions options)
            : base(options)
        {
        }

        public CarDealerContext()
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<PartCar> PartCars { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-2DFP0EC\SQLEXPRESS;Database=CarDealer;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PartCar>(e =>
            {
                e.HasKey(k => new { k.CarId, k.PartId });

                e.HasOne(x => x.Car)
                .WithMany(y => y.PartCars)
                .HasForeignKey(x => x.CarId);

                e.HasOne(x => x.Part)
                .WithMany(y => y.PartCars)
                .HasForeignKey(x => x.PartId);
            });

            modelBuilder.Entity<Part>(e =>
            {
                e.HasOne(x => x.Supplier)
                .WithMany(y => y.Parts)
                .HasForeignKey(x => x.SupplierId);
            });

            modelBuilder.Entity<Sale>(e =>
            {
                e.HasOne(x => x.Car)
                .WithMany(y => y.Sales)
                .HasForeignKey(x => x.CarId);

                e.HasOne(x => x.Customer)
                .WithMany(y => y.Sales)
                .HasForeignKey(x => x.CustomerId);
            });
        }
    }
}
