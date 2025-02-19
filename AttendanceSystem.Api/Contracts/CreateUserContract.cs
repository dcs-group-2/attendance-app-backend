using System.Text.Json.Serialization;

namespace AttendanceSystem.Api.Contracts;

public class CreateUserContract
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter<UserType>))]
    public required UserType Type { get; set; }
}

public enum UserType
{
    Student,
    Teacher,
    Administrator
}
