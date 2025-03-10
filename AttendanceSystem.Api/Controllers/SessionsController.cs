using AttendanceSystem.Api.Contracts;
using AttendanceSystem.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Domain.Services;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using static AttendanceSystem.Api.Roles;

using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace AttendanceSystem.Api.Controllers;

public class SessionsController : BaseController
{
    private readonly ILogger<SessionsController> _logger;
    private readonly AttendanceService _attendanceService;
    private readonly CourseService _courseService;

    public SessionsController(ILogger<SessionsController> logger, AttendanceService attendanceService, AuthenticationService authenticationService, UserService userService, CourseService courseService) : base(authenticationService, userService)
    {
        _logger = logger;
        _attendanceService = attendanceService;
        _courseService = courseService;
    }

    private async Task AssertCourseAuthorization(string courseId, string userId)
    {
        // Check if the user is allowed to retrieve the session.
        if(!await _courseService.UserCanAccessCourse(courseId, userId))
        {
            throw new UnauthorizedAccessException("The user is not allowed to retrieve the session.");
        };
    }

    [Function( $"{nameof(SessionsController)}-{nameof(GetAllSessions)}")]
    public async Task<IActionResult> GetAllSessions([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route="courses/{courseId}/sessions")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx, string courseId)
    {
        // Authorize
        await AssertAuthentication(ctx, AllowAll);
        await AssertCourseAuthorization(courseId, GetUserId(ctx));
        
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var sessions = await _attendanceService.GetSessions(courseId);

        // Replace with actual logic from AttendanceService
        return new OkObjectResult(sessions);
    }

    [Function( $"{nameof(SessionsController)}-{nameof(CreateNewSession)}")]
    public async Task<IActionResult> CreateNewSession([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route="courses/{courseId}/sessions")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx, string courseId, [FromBody] CreateSessionContract contract)
    {
        // Authorize
        await AssertAuthentication(ctx, AllowAll);

        _logger.LogInformation("C# HTTP trigger function processed a request.");

        Session session = await _attendanceService.CreateSession(courseId, contract.StartDate, contract.EndDate, []);

        return new OkObjectResult(session);
    }

    [Function( $"{nameof(SessionsController)}-{nameof(GetSessionInfo)}")]
    public async Task<IActionResult> GetSessionInfo([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route="courses/{courseId}/sessions/{sessionId:guid}")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx, string courseId, Guid sessionId)
    {
        // Authorize
        await AssertAuthentication(ctx, AllowAll);
        await AssertCourseAuthorization(courseId, GetUserId(ctx));

        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var session = await _attendanceService.GetSession(sessionId);
        return new OkObjectResult(session);
    }

    [Function( $"{nameof(SessionsController)}-{nameof(ConfirmStudentAttendance)}")]
    public async Task<IActionResult> ConfirmStudentAttendance([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route="courses/{courseId}/sessions/{sessionId:guid}/attendance")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx, string courseId, Guid sessionId, [FromBody] UpdateAttendanceContract contract)
    {
        // Authorize
        await AssertAuthentication(ctx, [Roles.Student]);
        await AssertCourseAuthorization(courseId, GetUserId(ctx));

        _logger.LogInformation("C# HTTP trigger function processed a request.");

        await _attendanceService.SetStudentAttendance(sessionId, contract.UserId, contract.Kind);

        return new NoContentResult();
    }

    [Function($"{nameof(SessionsController)}-{nameof(EditTeacherAttendance)}")]
    public async Task<IActionResult> EditTeacherAttendance([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "courses/{courseId}/sessions/{sessionId:guid}/teacherattendance")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx, string courseId, Guid sessionId, [FromBody] UpdateAttendanceContract contract)
    {
        // Authorize
        await AssertAuthentication(ctx, AllowElevated);

        // Check if the teacher can change the course
        if (!GetUserRoles(ctx).Contains(Roles.Admin))
        {
            await AssertCourseAuthorization(courseId, GetUserId(ctx));
        }

        _logger.LogInformation("C# HTTP trigger function processed a request.");
        await _attendanceService.SetTeacherApproval(sessionId, contract.UserId, contract.Kind);
        return new NoContentResult();
    }
    [Function( $"{nameof(SessionsController)}-{nameof(DeleteSession)}")]
    public async Task<IActionResult> DeleteSession([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route="courses/{courseId}/sessions/{sessionId:guid}")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx, string courseId, Guid sessionId)
    {
        // Authorize
        await AssertAuthentication(ctx, [Admin]);

        _logger.LogInformation("C# HTTP trigger function processed a request.");

        await _attendanceService.DeleteSession(sessionId);

        return new NoContentResult();
    }
}
