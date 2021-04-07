namespace Cinema.Data
{
    using Cinema.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class CinemaContext : DbContext
    {
        public CinemaContext()  { }

        public CinemaContext(DbContextOptions options)
            : base(options)   { }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Projection> Projections { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Seat> Seats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Projection>(entity =>
            {
                entity.HasOne(x => x.Movie)
                .WithMany(y => y.Projections)
                .HasForeignKey(x => x.MovieId);

                entity.HasOne(x => x.Hall)
                .WithMany(y => y.Projections)
                .HasForeignKey(x => x.HallId);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasOne(x => x.Customer)
                .WithMany(y => y.Tickets)
                .HasForeignKey(x => x.CustomerId);

                entity.HasOne(x => x.Projection)
                .WithMany(y => y.Tickets)
                .HasForeignKey(x => x.ProjectionId);
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.HasOne(x => x.Hall)
                .WithMany(y => y.Seats)
                .HasForeignKey(x => x.HallId);
            });
        }
    }
}