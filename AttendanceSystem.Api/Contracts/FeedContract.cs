using AttendanceSystem.Domain.Model;

namespace AttendanceSystem.Api.Contracts;

public class FeedContract
{
    public List<CourseDTO> Courses { get; set; }
    
    public List<SessionDTO> Sessions { get; set; }
}

public class CourseDTO
{
    public string Name { get; set; }
    public List<string> TeacherIds { get; set; }
    public string Department { get; set; }
}

public class SessionDTO
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public AttendanceRecordDto? Attendance { get; set; } = null!;
}

public class AttendanceRecordDto
{
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public AttendanceSubmission Status { get; set; }
    public AttendanceSubmission TeacherStatus { get; set; }
}