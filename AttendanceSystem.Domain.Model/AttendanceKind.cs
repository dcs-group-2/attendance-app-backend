namespace AttendanceSystem.Domain.Model;

public enum AttendanceKind
{
    Unknown = 0,
    Present,
    Absent,
    Late,
    Sick,
    LeftEarly,
}