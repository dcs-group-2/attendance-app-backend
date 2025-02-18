namespace AttendanceSystem.Api.Contracts;

public class CreateUserContract
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }


}
