﻿using AttendanceSystem.Data;
using AttendanceSystem.Domain.Model;
using AttendanceSystem.Domain.Model.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Domain.Services;

public class AttendanceService
{
    private readonly CoursesContext _context;

    public AttendanceService(CoursesContext context)
    {
        _context = context;
    }

    public async Task<Session> CreateSession(string courseId, DateTime startdate, DateTime enddate, List<string> students)
    {
        // Get the course to see if it is available
        Course course = await _context.Courses.FindAsync(courseId) ?? throw new EntityNotFoundException("Course not found");
        if (course is null) throw new EntityNotFoundException("Course not found");

        Session session = new Session(course, students, startdate, enddate);

        _context.Sessions.Add(session);
        course.Sessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task<List<Session>> GetSessions(string courseId)
    {
        return await _context.Sessions.Where(s => s.Course.Id == courseId).ToListAsync();
    }

    public async Task<List<Session>> GetUpcomingSessionsForUser(string userId)
    {
        return await _context.Sessions
            .Include(s => s.Course)
            .Where(s => s.Course.Students.Contains(userId) || s.Course.Teachers.Contains(userId)).ToListAsync();
    }

    public async Task<Session> GetSession(Guid sessionId)
    {
        return await _context.Sessions.FindAsync(sessionId) ?? throw new ArgumentException("Session not found", nameof(sessionId));
    }

    public async Task<Session> GetSessionWithRegister(Guid sessionId)
    {
        return await _context.Sessions.Include(s => s.Register).FirstOrDefaultAsync(s => s.Id == sessionId) ?? throw new ArgumentException("Session not found", nameof(sessionId));
    }

    public async Task DeleteSession(Guid sessionId)
    {
        var session = await GetSession(sessionId);
        _context.Sessions.Remove(session);
        await _context.SaveChangesAsync();
    }

    public async Task SetStudentAttendance(Guid sessionId, string studentId, AttendanceKind kind)
    {
        var session = await GetSessionWithRegister(sessionId);
        session.SetStudentAttendance(studentId, kind);
        await _context.SaveChangesAsync();
    }

    public async Task SetTeacherApproval(Guid sessionId, string studentId, AttendanceKind kind)
    {
        var session = await GetSessionWithRegister(sessionId);
        session.SetTeacherApproval(studentId, kind);
        await _context.SaveChangesAsync();
    }

    public async Task<AttendanceRecord?> GetSessionStatus(Guid sessionId, string userId)
    {
        AttendanceRecord? attendanceRecord =
            await (from session in _context.Sessions
            from record in session.Register
            where record.SessionId == sessionId && record.StudentId == userId
            select record).FirstOrDefaultAsync();

        return attendanceRecord;
    }

    public async Task<Dictionary<Guid, AttendanceRecord>> GetSessionStatuses(string userId, List<Guid> toList)
    {
        var records =
            from session in _context.Sessions
            from record in session.Register
            where toList.Contains(session.Id) && record.StudentId == userId
            select record;

        var recordsList = await records.ToListAsync();

        return recordsList.ToDictionary(r => r.SessionId, r => r);
    }
}
