using Microsoft.Extensions.DependencyInjection;
using AttendanceSystem.Domain.Services;
using AttendanceSystem.Domain.Services.Tools;

namespace AttendanceSystem.Api;

public static class Services
{
    public static IServiceCollection AddServices(this IServiceCollection sp)
    {
        sp.AddScoped<CourseService>();
        sp.AddScoped<AttendanceService>();
        sp.AddScoped<UserService>();
        sp.AddScoped<MockDataGenerator>();

        sp.AddSwagger();

        return sp;
    }
}
