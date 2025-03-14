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
    [ProducesResponseType<List<SessionDTO>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUpcomingSessions([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route="feed")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx)
    {
        await AssertAuthentication(ctx, AllowAll);

        User user = await GetUser(ctx);
        string userId = user.Id;

        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var sessions = await _attendanceService.GetUpcomingSessionsForUser(userId);

        Dictionary<Guid, AttendanceRecord> attendanceStatus = [];
        if (user is Student)
        {
            attendanceStatus = await _attendanceService.GetSessionStatuses(userId, sessions.Select(s => s.Id).ToList());
        }

        // Create the feed
        List<SessionDTO> sessionContracts = new();
        foreach (Session session in sessions)
        {
            AttendanceRecord record = user is Student ? attendanceStatus[session.Id] : null!;
            sessionContracts.Add(new SessionDTO
                {
                    Id = session.Id,
                    CourseName = session.Course.Name,
                    CourseId = session.Course.Id,
                    StartDate = session.StartTime,
                    EndDate = session.EndTime,
                    Attendance = user is Student
                        ? new AttendanceRecordDto()
                        {
                            Status = record.StudentSubmission,
                            TeacherStatus = record.StudentSubmission,
                            StudentId = record.StudentId,
                            StudentName = user.Name,
                        }
                        : null!,
                }
            );
        }

        return new OkObjectResult(sessionContracts);
    }
}
