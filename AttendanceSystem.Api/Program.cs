using AttendanceSystem.Api;
using AttendanceSystem.Api.Middleware;
using AttendanceSystem.Data;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using AttendanceSystem.Domain.Services.Tools;
using Microsoft.Azure.Functions.Worker;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.UseMiddleware<ExceptionToErrorCodeHandler>();
builder.UseMiddleware<AuthenticationHandler>();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.AddServices();
builder.Services.AddMvc();

builder.Services.AddDbContext<CoursesContext>(options =>
{
    string? connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

    if (string.IsNullOrEmpty(connectionString)) throw new InvalidOperationException("SqlConnectionString is not set.");

    options.UseAzureSql(connectionString);
});

IHost host = builder.Build();

// Configure the database
using (var scope = host.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CoursesContext>();

    var mockDataGenerator = scope.ServiceProvider.GetRequiredService<MockDataGenerator>();

    if (builder.Environment.IsDevelopment())
    {
        // If we are in development, start with a fresh database on every launch
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        await mockDataGenerator.GenerateMockData();
    }
    else
    {
        context.Database.Migrate();
    }
}

// Open the server
host.Run();
