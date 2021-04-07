using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }
        public StudentSystemContext(DbContextOptions options) 
            : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.StudentId);

                entity.Property(s => s.Name)
                .HasMaxLength(100)
                .IsUnicode(true)
                .IsRequired(true);

                entity.Property(s => s.PhoneNumber)
                .HasMaxLength(10)
                .IsFixedLength(true)
                .IsUnicode(false)
                .IsRequired(false);

                entity.Property(s => s.RegisteredOn)
                .HasColumnType("DATE");

                entity.Property(s => s.Birthday)
                .HasColumnType("DATE");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(c => c.CourseId);

                entity.Property(c => c.Name)
                .HasMaxLength(80)
                .IsUnicode(true)
                .IsRequired(true);

                entity.Property(c => c.Description)
                .IsRequired(false)
                .IsUnicode(true);

                entity.Property(c => c.StartDate)
                .HasColumnType("DATE");

                entity.Property(c => c.EndDate)
                .HasColumnType("DATE");

                //entity.Property(c => c.Price);
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasKey(r => r.ResourceId);

                entity.Property(r => r.Name)
                .HasMaxLength(50)
                .IsUnicode(true)
                .IsRequired(true);

                entity.Property(r => r.Url)
                .IsRequired(true)
                .IsUnicode(false);

                entity.HasOne(r => r.Course)
                .WithMany(c => c.Resources)
                .HasForeignKey(r => r.CourseId);
            });

            modelBuilder.Entity<Homework>(entity =>
            {
                entity.HasKey(h => h.HomeworkId);

                entity.Property(h => h.Content)
                .IsRequired(true)
                .IsUnicode(false);

                entity.Property(h => h.SubmissionTime)
                .HasColumnType("SMALLDATETIME");

                entity.HasOne(h => h.Student)
                .WithMany(s => s.HomeworkSubmissions)
                .HasForeignKey(h => h.StudentId);

                entity.HasOne(h => h.Course)
                .WithMany(c => c.HomeworkSubmissions)
                .HasForeignKey(h => h.CourseId);
            });

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(sc => new { sc.StudentId, sc.CourseId });

                entity.HasOne(sc => sc.Student)
                .WithMany(s => s.CourseEnrollments)
                .HasForeignKey(sc => sc.StudentId);

                entity.HasOne(sc => sc.Course)
                .WithMany(c => c.StudentsEnrolled)
                .HasForeignKey(sc => sc.CourseId);
            });
        }
    }
}
