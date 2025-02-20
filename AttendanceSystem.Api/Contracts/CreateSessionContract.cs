namespace AttendanceSystem.Api.Contracts;

public class CreateSessionContract
{
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
}