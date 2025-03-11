using System.Diagnostics.CodeAnalysis;
using AttendanceSystem.Domain.Model;

namespace AttendanceSystem.Api.Contracts;

public class GetAttendanceListContract
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required Course Course { get; set; }
    
    public required DateTime StartTime { get; set; }
    
    public required DateTime EndTime { get; set; }

    public required ICollection<ExtendedAttendanceRecord> Register { get; set; } = null!;
}

public record ExtendedAttendanceRecord() : AttendanceRecord
{
    [SetsRequiredMembers]
    public ExtendedAttendanceRecord(AttendanceRecord attendanceRecord) : this()
    {
        SessionId = attendanceRecord.SessionId;
        StudentId = attendanceRecord.StudentId;
        StudentSubmission = attendanceRecord.StudentSubmission;
        TeacherSubmission = attendanceRecord.TeacherSubmission;
    }
    
    public string StudentName { get; set; }
}