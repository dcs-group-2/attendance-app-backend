using AttendanceSystem.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Data;

public class CoursesContext : DbContext
{
    public CoursesContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }

    public DbSet<Teacher> Teachers { get; set; }

    public DbSet<Course> Courses { get; set; }

    public DbSet<Session> Sessions { get; set; }

    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>(e =>
            {
                e.HasKey(d => d.Name);
            }
        );

        modelBuilder.Entity<User>(e =>
        {
            e.HasDiscriminator<string>("Role")
                .HasValue<Student>("Student")
                .HasValue<Teacher>("Teacher")
                .HasValue<Administrator>("Administrator");
        });

        modelBuilder.Entity<AttendanceRecord>(e =>
        {
            e.HasKey(r => new {r.Session, r.Student});
        });
    }
}
