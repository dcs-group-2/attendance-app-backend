namespace AttendanceSystem.Domain.Model;

public class Department
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; }
}