using AttendanceSystem.Data;
using AttendanceSystem.Domain.Model;
using AttendanceSystem.Domain.Model.Exceptions;
using AttendanceSystem.Domain.Services.Alterations;
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

    public async Task<Course> CreateNewCourse(string id, string name, string departmentId, List<string> teacherIds)
    {
        var course = new Course(id, name, departmentId, teacherIds);
        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task<Course> GetCourse(string courseId)
    {
        return await _context.Courses.FindAsync(courseId) ?? throw new EntityNotFoundException("Course not found");
    }

    public async Task<Course> ConfigureCourse(string courseId, CourseAlteration alteration)
    {
        // Extract course details from the request and update the course
        var course = await GetCourse(courseId);

        course.Name = alteration.Name ?? course.Name;
        course.Department = alteration.Department ?? course.Department;

        await _context.SaveChangesAsync();
        return course;
    }

    public async Task DeleteCourse(string courseId)
    {
        var course = await GetCourse(courseId);
        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
    }

    public async Task EnrollUser(string courseId, string userId)
    {
        // Extract user details from the request and enroll the user in the course
        var course = await GetCourse(courseId);
        course.Students.Add(userId);
        await _context.SaveChangesAsync();
    }
}
