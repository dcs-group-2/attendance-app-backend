using System.Text.Json.Serialization;
using AttendanceSystem.Domain.Model;

namespace AttendanceSystem.Api.Contracts;

public class UpdateAttendanceContract
{
    public required List<UpdateAttendanceContractItem> Attendance { get; set; }
}

public class UpdateAttendanceContractItem
{
    public required string? UserId { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter<AttendanceKind>))]
    public required AttendanceKind Kind { get; init; }
}