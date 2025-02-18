namespace AttendanceSystem.Api.Contracts;

public class CreateCourseContract
{
    public required string name;
    public required string description;
    public required string departmentId;
    public required List<string> teacherIds;
}
