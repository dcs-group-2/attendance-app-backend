using Microsoft.Extensions.DependencyInjection;
using AttendanceSystem.Domain.Services;

namespace AttendanceSystem.Api;

public static class Services
{
    public static IServiceCollection AddServices(this IServiceCollection sp)
    {
        sp.AddScoped<CourseService>();
        sp.AddScoped<AttendanceService>();
        sp.AddScoped<UserService>();
        
        return sp;
    }
}
