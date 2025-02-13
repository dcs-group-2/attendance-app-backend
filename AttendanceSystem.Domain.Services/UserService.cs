using AttendanceSystem.Data;
using AttendanceSystem.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Domain.Services;

public class UserService
{
    private readonly CoursesContext _context;
    
    public UserService(CoursesContext context)
    {
        _context = context;
    }
    
    public async Task<User> GetUser(Guid userId)
    {
        return await _context.Students.SingleAsync(u => u.Id == userId);
    }

    public async Task<List<User>> GetUsers()
    {
        return await _context.Students.ToListAsync<User>();
    }

    public async Task<User> CreateTeacher(Guid id, string name, string email)
    {
        return new Teacher()
        {
            Id = id,
            Name = name,
            Email = email,
        };
    }
    
    public async Task<User> CreateStudent(Guid id, string name, string email)
    {
        return new Student()
        {
            Id = id,
            Name = name,
            Email = email,
        };
    }
    
    public async Task<User> CreateAdministrator(Guid id, string name, string email)
    {
        return new Administrator()
        {
            Id = id,
            Name = name,
            Email = email,
        };
    }
}