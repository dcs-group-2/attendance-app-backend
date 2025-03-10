using AttendanceSystem.Data;
using AttendanceSystem.Domain.Model;
using AttendanceSystem.Domain.Model.Exceptions;
using AttendanceSystem.Domain.Services.Alterations;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Domain.Services;

public class UserService
{
    private readonly CoursesContext _context;

    public UserService(CoursesContext context)
    {
        _context = context;
    }

    public async Task<User> GetUser(string userId)
    {
        return await _context.Students.FindAsync(userId) ?? throw new EntityNotFoundException("User not found");
    }

    public async Task<List<User>> GetAllStudents()
    {
        return await _context.Students.ToListAsync<User>();
    }

    public async Task<User> CreateTeacher(string id, string name, string email)
    {
        User user = new Teacher()
        {
            Id = id,
            Name = name,
            Email = email,
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> CreateStudent(string id, string name, string email)
    {
        User user = new Student()
        {
            Id = id,
            Name = name,
            Email = email,
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> CreateAdministrator(string id, string name, string email)
    {
        User user = new Administrator()
        {
            Id = id,
            Name = name,
            Email = email,
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> EditUser(string userId, UserAlteration alteration)
    {
        if (alteration?.Name is null && alteration?.Email is null)
        {
            throw new ArgumentException("At least one property must be provided to edit the user");
        }

        var user = await GetUser(userId);

        user.Name = alteration.Name ?? user.Name;
        user.Email = alteration.Email ?? user.Email;

        await _context.SaveChangesAsync();

        return user;
    }

    public async Task DeleteUser(string userId)
    {
        var user = await GetUser(userId);
        _context.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetUsers(List<string> users)
    {
        return await _context.Users.Where(u => users.Contains(u.Id)).ToListAsync();
    }
}
