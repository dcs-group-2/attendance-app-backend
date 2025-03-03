namespace AttendanceSystem.Domain.Model;

public record AttendanceRecord
{
    public required SessionId SessionId { get; set; }
    public required StudentId StudentId { get; set; }
    public required AttendanceKind StudentAttendance { get; set; }
    public required DateTime? StudentSubmitted { get; set; }
    public required AttendanceKind TeacherAttendance { get; set; }
    public required DateTime? TeacherSubmitted { get; set; }
}
