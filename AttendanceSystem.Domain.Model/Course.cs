namespace AttendanceSystem.Domain.Model;

public class Course
{
    public CourseId Id { get; private set; }

    public string Name { get; internal set; }

    public string Department { get; internal set; }

    public List<string> Teachers { get; internal set; }

    public List<string> Students { get; internal set; } = [];


    public List<Session> Sessions { get; internal set; } = [];

#pragma warning disable CS8618, CS9264
    private Course() { }
#pragma warning restore CS8618, CS9264

    public Course(CourseId id, string name, string department, List<string> teachers)
    {
        if (teachers.Count == 0)
        {
            throw new ArgumentException("A course must have at least one teacher", nameof(teachers));
        }

        Id = id;
        Name = name;
        Department = department;
        Teachers = teachers;
    }
}
