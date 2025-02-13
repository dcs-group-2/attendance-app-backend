using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Api.Controllers;

public class SessionsController
{
    private readonly ILogger<SessionsController> _logger;
    
    public SessionsController(ILogger<SessionsController> logger)
    {
        _logger = logger;
    }
    
    [Function( $"{nameof(SessionsController)}-{nameof(GetAllSessions)}")]
    public IActionResult GetAllSessions([HttpTrigger(AuthorizationLevel.User, "get", Route="courses/{courseId:guid}/sessions")] HttpRequest req, Guid courseId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }
    
    [Function( $"{nameof(SessionsController)}-{nameof(CreateNewSession)}")]
    public IActionResult CreateNewSession([HttpTrigger(AuthorizationLevel.User, "post", Route="courses/{courseId:guid}/sessions")] HttpRequest req, Guid courseId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }
    
    [Function( $"{nameof(SessionsController)}-{nameof(GetSessionInfo)}")]
    public IActionResult GetSessionInfo([HttpTrigger(AuthorizationLevel.User, "get", Route="courses/{courseId:guid}/sessions/{sessionId:guid}")] HttpRequest req, Guid courseId, Guid sessionId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }
    
    [Function( $"{nameof(SessionsController)}-{nameof(EditAttendance)}")]
    public IActionResult EditAttendance([HttpTrigger(AuthorizationLevel.User, "put", Route="courses/{courseId:guid}/sessions/{sessionId:guid}")] HttpRequest req, Guid courseId, Guid sessionId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }
    
    [Function( $"{nameof(SessionsController)}-{nameof(DeleteSession)}")]
    public IActionResult DeleteSession([HttpTrigger(AuthorizationLevel.User, "delete", Route="courses/{courseId:guid}/sessions/{sessionId:guid}")] HttpRequest req, Guid courseId, Guid sessionId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }
    
    [Function( $"{nameof(SessionsController)}-{nameof(GetCourseAttendanceReport)}")]
    public IActionResult GetCourseAttendanceReport(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = "courses/{courseId:guid}/attendance")] HttpRequest req,
        Guid courseId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }

    [Function( $"{nameof(SessionsController)}-{nameof(GetAttendanceReport)}")]
    public IActionResult GetAttendanceReport(
        [HttpTrigger(AuthorizationLevel.User, "get",
            Route = "courses/{courseId:guid}/sessions/{sessionId:guid}/attendance")]
        HttpRequest req, Guid courseId, Guid sessionId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }
}
