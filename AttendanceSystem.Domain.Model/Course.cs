namespace AttendanceSystem.Domain.Model;

public class Course
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public Department Department { get; private set; }
    
    public List<Teacher> Teachers { get; private set; } = [];

    
    public List<Student> Students { get; private set; } = [];
    
    
    public List<Session> Sessions { get; private set; } = [];
    
    public string Name { get; private set; }

    public Course(string name, Department department, List<Teacher> teachers)
    {
        if (teachers.Count == 0)
        {
            throw new ArgumentException("A course must have at least one teacher", nameof(teachers));
        }
        
        Name = name;
        Department = department;
        Teachers = teachers;
    }
}