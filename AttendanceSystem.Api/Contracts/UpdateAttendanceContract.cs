using System.Text.Json.Serialization;
using AttendanceSystem.Domain.Model;

namespace AttendanceSystem.Api.Contracts;

public class UpdateAttendanceContract
{
    public required string UserId { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter<AttendanceKind>))]
    public required AttendanceKind Kind { get; set; }
}