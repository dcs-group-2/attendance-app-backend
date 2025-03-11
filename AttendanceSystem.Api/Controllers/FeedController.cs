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
    public async Task<IActionResult> GetUpcomingSessions([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route="feed")] HttpRequest req, FunctionContext ctx)
    {
        await AssertAuthentication(ctx, AllowAll);

        string userId = GetUserId(ctx);

        bool isTeacher = GetUserRoles(ctx).Contains(Roles.Teacher) || GetUserRoles(ctx).Contains(Roles.Admin);

        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var courses = new List<Session>();
        if (isTeacher)
        {
            courses = await _attendanceService.GetUpcomingSessionsForTeacher(userId);
            return new OkObjectResult(courses);
        }

        courses = await _attendanceService.GetUpcomingSessionsForUser(userId);
        return new OkObjectResult(courses);
    }


}
