namespace CarDealer.Data
{
    using Microsoft.EntityFrameworkCore;

    using Models;

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

                e.HasOne(p => p.Car)
                .WithMany(c => c.PartCars)
                .HasForeignKey(p => p.CarId);

                e.HasOne(p => p.Part)
                .WithMany(p => p.PartCars)
                .HasForeignKey(p => p.PartId);
            });

            modelBuilder.Entity<Part>(e =>
            {
                e.HasOne(p => p.Supplier)
                .WithMany(s => s.Parts)
                .HasForeignKey(p => p.SupplierId);
            });

            modelBuilder.Entity<Sale>(e =>
            {
                e.HasOne(s => s.Customer)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CustomerId);

                e.HasOne(s => s.Car)
                .WithMany(c => c.Sales)
                .HasForeignKey(s => s.CarId);
            });
        }
    }
}
