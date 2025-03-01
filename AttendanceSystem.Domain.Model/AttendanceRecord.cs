﻿namespace AttendanceSystem.Domain.Model;

public record AttendanceRecord
{
    public required SessionId SessionId { get; set; }
    public required StudentId StudentId { get; set; }
    public required AttendanceKind Record { get; set; }
}
