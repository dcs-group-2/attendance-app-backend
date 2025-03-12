using AttendanceSystem.Api.Contracts;
using AttendanceSystem.Domain.Model;
using AttendanceSystem.Domain.Services;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using static AttendanceSystem.Api.Roles;

namespace AttendanceSystem.Api.Controllers;

public class FeedController : BaseController
{
    private readonly ILogger<SessionsController> _logger;
    private readonly AttendanceService _attendanceService;

    public FeedController(ILogger<SessionsController> logger, AttendanceService attendanceService, AuthenticationService authenticationService, UserService userService) : base(authenticationService, userService)
    {
        _logger = logger;
        _attendanceService = attendanceService;
    }

    /// <summary>
    /// Gets the upcoming session for the logged-in user
    /// </summary>
    /// <returns>The upcoming session which the user needs to attend.</returns>
    /// <response code="200">Successful</response>
    [Function( $"{nameof(FeedController)}-{nameof(GetUpcomingSessions)}")]
    [ProducesResponseType<List<Session>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUpcomingSessions([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route="feed")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx)
    {
        await AssertAuthentication(ctx, AllowAll);

        User user = await GetUser(ctx);
        string userId = user.Id;

        bool isTeacher = GetUserRoles(ctx).Contains(Roles.Teacher) || GetUserRoles(ctx).Contains(Roles.Admin);

        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var sessions = await _attendanceService.GetUpcomingSessionsForUser(userId);
        
        // Get the courses
        var courses = sessions.Select(s => s.Course).Distinct();
        
        // Create the feed
        List<SessionDTO> sessionDTOs = new();
        foreach (Session session in sessions)
        {
            var record = await _attendanceService.GetSessionStatus(session.Id, userId);
            sessionDTOs.Add(new SessionDTO()
            {
                Id = session.Id,
                StartDate = session.StartTime,
                EndDate = session.EndTime,
                Attendance = user is Student ? new AttendanceRecordDto() {
                    Status = record.StudentSubmission,
                    TeacherStatus = record.StudentSubmission,
                    StudentId = record.StudentId,
                    StudentName = user.Name,
                } : null!,
            });
        }

        FeedContract contract = new FeedContract()
        {
            Courses = courses.Select(c => new CourseDTO()
                { Department = c.Department, Name = c.Name, TeacherIds = c.Teachers }).ToList(),
            Sessions = sessionDTOs,
        };

        return new OkObjectResult(contract);
    }
}
