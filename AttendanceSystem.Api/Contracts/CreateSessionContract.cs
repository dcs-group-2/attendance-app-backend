namespace AttendanceSystem.Api.Contracts;

public class CreateSessionContract
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}