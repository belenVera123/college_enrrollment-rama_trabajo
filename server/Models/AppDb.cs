using Microsoft.EntityFrameworkCore;

namespace WebSqliteApp.Models;

public class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    public DbSet<Assessment> Assessments { get; set; }
    public DbSet<StudentScore> StudentScores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Student>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Enrollment>()
            .HasIndex(u => u.StudentId);

        modelBuilder.Entity<Enrollment>()
            .HasIndex(u => u.CourseId);
    }
}