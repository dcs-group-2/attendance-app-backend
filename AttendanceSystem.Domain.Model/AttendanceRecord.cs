namespace AttendanceSystem.Domain.Model;

public record AttendanceRecord
{
    public required SessionId SessionId { get; set; }
    public required StudentId StudentId { get; set; }

    public required AttendanceSubmission StudentSubmission { get; set; }
    public required AttendanceSubmission TeacherSubmission { get; set; }

}