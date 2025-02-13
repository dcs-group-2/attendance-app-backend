using AttendanceSystem.Data;
using AttendanceSystem.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Domain.Services;

public class AttendanceService
{
    private readonly CoursesContext _context;
    
    public AttendanceService(CoursesContext context)
    {
        _context = context;
    }

    public async Task<Session> GetSession(Guid sessionId)
    {
        return await _context.Sessions.SingleAsync(s => s.Id == sessionId);
    }
    
    public async Task SetAttendance(Guid sessionId, Guid studentId, AttendanceKind kind)
    {
        var session = await GetSession(sessionId);
        session.SetAttendance(studentId, kind);
        await _context.SaveChangesAsync();
    }
}