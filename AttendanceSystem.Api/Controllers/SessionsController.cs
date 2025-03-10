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
        await AssertAuthentication(ctx, [Admin]);
        
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        Session session = await _attendanceService.CreateSession(courseId, contract.StartDate, contract.EndDate, contract.Participants);    

        return new OkObjectResult(session);
    }

    /// <summary>
    /// Gets the information for a specific session.
    /// </summary>
    /// <remarks>
    /// When the user is a teacher, the register is available. When the user is a student, only their own attendance is available.
    /// </remarks>
    /// <param name="req">The http request of the function.</param>
    /// <param name="ctx">The context of the functions.</param>
    /// <param name="courseId">The id of the course.</param>
    /// <param name="sessionId">The id of the session.</param>
    /// <returns>The session information.</returns>
    [Function( $"{nameof(SessionsController)}-{nameof(GetSessionInfo)}")]
    [ProducesResponseType<GetAttendanceListContract>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSessionInfo([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route="courses/{courseId}/sessions/{sessionId:guid}")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx, string courseId, Guid sessionId)
    {
        // Authorize
        await AssertAuthentication(ctx, AllowAll);
        string userId = GetUserId(ctx);
        await AssertCourseAuthorization(courseId, userId);

        bool isTeacher = GetUserRoles(ctx).Contains(Roles.Teacher) || GetUserRoles(ctx).Contains(Roles.Admin);
        
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var session = await _attendanceService.GetSessionWithRegister(sessionId);
        var userProfiles = await _userService.GetUsers(session.Register.Select(r => r.StudentId).ToList());

        // Get the register for the session
        // If the user is a teacher, get all the students. If the user is a student, only get their own attendance.
        // Also, get the student name from the user profiles.
        var register = isTeacher ? session.Register : session.Register.Where(r => r.StudentId == userId);
        var extendedRegister = register.Select(r => new ExtendedAttendanceRecord(r)
        {
            StudentName = userProfiles.FirstOrDefault(u => u.Id == r.StudentId)?.Name!,
        });
        
        // Convert the session to a response contract
        var contract = new GetAttendanceListContract
        {
            Id = session.Id,
            Course = session.Course,
            StartTime = session.StartTime,
            EndTime = session.EndTime,
            Register = extendedRegister.ToList(),
        };
        
        return new OkObjectResult(contract);
    }

    [Function( $"{nameof(SessionsController)}-{nameof(SetAttendance)}")]
    public async Task<IActionResult> SetAttendance([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route="courses/{courseId}/sessions/{sessionId:guid}/attendance")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx, string courseId, Guid sessionId, [FromBody] UpdateAttendanceContract contract)
    {
        // Right now, we only accept one attendance record at a time.
        if (contract.Attendance.Count != 1)
        {
            return new BadRequestObjectResult("Only one attendance record can be updated at a time.");
        }
        
        var recordProcessRequest = contract.Attendance.First();
        
        // Authorize
        await AssertAuthentication(ctx, AllowAll);
        await AssertCourseAuthorization(courseId, GetUserId(ctx));

        _logger.LogInformation("C# HTTP trigger function processed a request.");

        // Based on the role, set the attendance as a student or as a teacher
        bool isTeacher = GetUserRoles(ctx).Contains(Roles.Teacher) || GetUserRoles(ctx).Contains(Roles.Admin);
        
        if (isTeacher)
        {
            if(recordProcessRequest.UserId is null)
            {
                return new BadRequestObjectResult("The user id is required for teacher attendance.");
            }
            
            await _attendanceService.SetTeacherApproval(sessionId, recordProcessRequest.UserId, recordProcessRequest.Kind);
        }
        else
        {
            await _attendanceService.SetStudentAttendance(sessionId, GetUserId(ctx), recordProcessRequest.Kind);
        }
        
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
