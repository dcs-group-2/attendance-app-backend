namespace AttendanceSystem.Domain.Model;

public record AttendanceRecord
{
    public required SessionId Session { get; set; }
    public required StudentId Student { get; set; }
    public required AttendanceKind Record { get; set; }
}
