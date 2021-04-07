namespace TeisterMask.Data
{
    using Microsoft.EntityFrameworkCore;
    using TeisterMask.Data.Models;

    public class TeisterMaskContext : DbContext
    {
        public TeisterMaskContext() { }

        public TeisterMaskContext(DbContextOptions options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<EmployeeTask> EmployeesTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasOne(x => x.Project)
                .WithMany(y => y.Tasks)
                .HasForeignKey(x => x.ProjectId);
            });

            modelBuilder.Entity<EmployeeTask>(entity =>
            {
                entity.HasKey(x => new { x.EmployeeId, x.TaskId });

                entity.HasOne(x => x.Employee)
                .WithMany(y => y.EmployeesTasks)
                .HasForeignKey(x => x.EmployeeId);

                entity.HasOne(x => x.Task)
                .WithMany(y => y.EmployeesTasks)
                .HasForeignKey(x => x.TaskId);
            });
        }
    }
}