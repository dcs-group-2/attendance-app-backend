namespace AttendanceSystem.Api.Contracts;

public class CreateCourseContract
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string DepartmentId { get; set; }
    public required List<string> TeacherIds { get; set; }
}
