namespace AttendanceSystem.Domain.Model;

public class Student : User
{
    public byte[]? FaceData { get; set; }
    
    bool HasBiometricData => FaceData is not null;
}