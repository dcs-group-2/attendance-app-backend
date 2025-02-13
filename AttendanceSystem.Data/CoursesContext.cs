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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}