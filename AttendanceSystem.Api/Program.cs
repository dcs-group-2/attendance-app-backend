using AttendanceSystem.Api;
using AttendanceSystem.Data;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetry()
//     .ConfigureFunctionsApplicationInsights();

builder.Services.AddServices();
builder.Services.AddMvc();

builder.Services.AddDbContext<CoursesContext>(options =>
{
    string? connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

    if (string.IsNullOrEmpty(connectionString)) throw new InvalidOperationException("SqlConnectionString is not set.");

    options.UseAzureSql(connectionString);
});

builder.Build().Run();
