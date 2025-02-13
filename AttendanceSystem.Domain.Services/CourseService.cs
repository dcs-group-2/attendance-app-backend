using AttendanceSystem.Data;
using AttendanceSystem.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Domain.Services;

public class CourseService
{
    private readonly CoursesContext _context;

    public CourseService(CoursesContext context)
    {
        _context = context;
    }

    public async Task<List<Course>> GetAllCourses()
    {
        return await _context.Courses.ToListAsync();
    }

    public async Task<Course> CreateNewCourse(HttpRequest req)
    {
        // Extract course details from the request and create a new course
        var course = new Course(/* parameters */);
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task<Course> GetCourse(Guid courseId)
    {
        return await _context.Courses.SingleOrDefaultAsync(c => c.Id == courseId);
    }

    public async Task<Course> ConfigureCourse(HttpRequest req, Guid courseId)
    {
        // Extract course details from the request and update the course
        var course = await GetCourse(courseId);
        // Update course properties
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task DeleteCourse(Guid courseId)
    {
        var course = await GetCourse(courseId);
        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
    }

    public async Task<User> EnrollUser(HttpRequest req, Guid courseId)
    {
        // Extract user details from the request and enroll the user in the course
        var course = await GetCourse(courseId);
        var user = new User(/* parameters */);
        course.Students.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
}
