using AttendanceSystem.Domain.Model;

namespace AttendanceSystem.Api.Contracts;

public record SessionDTO
{
    public required Guid Id { get; set; }

    public required string CourseName { get; set; }

    public required string CourseId { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public AttendanceRecordDto? Attendance { get; set; } = null!;
}

public record AttendanceRecordDto
{
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public AttendanceSubmission Status { get; set; }
    public AttendanceSubmission TeacherStatus { get; set; }
}
