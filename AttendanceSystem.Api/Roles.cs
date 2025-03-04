namespace AttendanceSystem.Api;

public static class Roles
{
    public const string Admin = "role.admin";
    public const string Teacher = "role.teacher";
    public const string Student = "role.student";
    
    public static string[] AllowElevated = [Admin, Teacher];
    public static string[] AllowAll = [Admin, Teacher, Student];
}