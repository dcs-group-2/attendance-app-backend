namespace AttendanceSystem.Domain.Model;

public record AttendanceRecord
{
    public required Session Session { get; set; }
    public required Student Student { get; set; }
    public required AttendanceKind Record { get; set; }
}