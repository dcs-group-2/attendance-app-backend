﻿using AttendanceSystem.Domain.Model.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using static AttendanceSystem.Domain.Model.AttendanceKind;

namespace AttendanceSystem.Domain.Model;

public class Session
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required Course Course { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }

    public ICollection<AttendanceRecord> Register { get; }

    [SetsRequiredMembers]
    private Session() { }

    [SetsRequiredMembers]
    public Session(Course course, List<StudentId> students, DateTime startTime, DateTime endTime)
    {
        Course = course;
        StartTime = startTime;
        EndTime = endTime;

        // Check if all students are currently signed-up for the course
        if (!students.All(s => course.Students.Contains(s)))
        {
            throw new ArgumentException("All students should be enrolled in the course to create a session.");
        }

        // Create the attendance records for all students
        Register = students.Select(s => new AttendanceRecord
        {
            SessionId = Id,
            StudentId = s,
            StudentSubmission = new AttendanceSubmission(),
            TeacherSubmission = new AttendanceSubmission(),
        }).ToList();
    }

    public void SetStudentAttendance(StudentId studentId, AttendanceKind attendance)
    {
        var record = Register.FirstOrDefault(r => r.StudentId == studentId);
        if (record is null)
        {
            throw new EntityNotFoundException("Student is not registered for this session.");
        }
        record.StudentSubmission.Attendance = attendance;
    }

    public void SetTeacherApproval(StudentId studentId, AttendanceKind attendance)
    {
        var record = Register.FirstOrDefault(r => r.StudentId == studentId);
        if (record is null)
        {
            throw new EntityNotFoundException("Student is not registered for this session.");
        }

        record.TeacherSubmission.Attendance = attendance;
    }

}
