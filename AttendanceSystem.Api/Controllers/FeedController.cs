using AttendanceSystem.Domain.Model;
using AttendanceSystem.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Api.Controllers;

public class FeedController
{
    private readonly ILogger<SessionsController> _logger;
    private readonly AttendanceService _attendanceService;

    public FeedController(ILogger<SessionsController> logger, AttendanceService attendanceService)
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
    public async Task<IActionResult> GetUpcomingSessions([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route="feed")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        // Replace with logic getting the actual user
        var userId = "deadbeef-dead-beef-dead-beefdeadbeef";

        var courses = await _attendanceService.GetUpcomingSessionsForUser(userId);
        return new OkObjectResult(courses);
    }
}
