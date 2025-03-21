﻿using System.Text.Json.Serialization;
using AttendanceSystem.Domain.Model;

namespace AttendanceSystem.Api.Contracts;

public class UpdateAttendanceContractItem
{
    public string? UserId { get; init; } = null!;

    [JsonConverter(typeof(JsonStringEnumConverter<AttendanceKind>))]
    public required AttendanceKind Kind { get; init; }
}
