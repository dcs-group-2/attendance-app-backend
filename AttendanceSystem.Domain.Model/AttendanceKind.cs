using System.Text.Json.Serialization;

namespace AttendanceSystem.Domain.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AttendanceKind
{
    Unknown = 0,
    Present,
    Absent,
    Late,
    Sick,
    LeftEarly,
}