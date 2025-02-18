using AttendanceSystem.Api.Contracts;
using AttendanceSystem.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Domain.Services;

namespace AttendanceSystem.Api.Controllers;

public class SessionsController
{
    private readonly ILogger<SessionsController> _logger;
    private readonly AttendanceService _attendanceService;
    
    public SessionsController(ILogger<SessionsController> logger, AttendanceService attendanceService)
    {
        _logger = logger;
        _attendanceService = attendanceService;
    }
    
    [Function( $"{nameof(SessionsController)}-{nameof(GetAllSessions)}")]
    public async Task<IActionResult> GetAllSessions([HttpTrigger(AuthorizationLevel.User, "get", Route="courses/{courseId:string}/sessions")] HttpRequest req, string courseId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var sessions = await _attendanceService.GetSessions(courseId);
        
        // Replace with actual logic from AttendanceService
        return new OkObjectResult(sessions);
    }
    
    [Function( $"{nameof(SessionsController)}-{nameof(CreateNewSession)}")]
    public async Task<IActionResult> CreateNewSession([HttpTrigger(AuthorizationLevel.User, "post", Route="courses/{courseId:string}/sessions")] HttpRequest req, string courseId, [FromBody] CreateSessionContract contract)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        
        Session session = await _attendanceService.CreateSession(courseId, contract.StartDate, contract.EndDate);
        
        return new OkObjectResult(session);
    }
    
    [Function( $"{nameof(SessionsController)}-{nameof(GetSessionInfo)}")]
    public async Task<IActionResult> GetSessionInfo([HttpTrigger(AuthorizationLevel.User, "get", Route="courses/{courseId:string}/sessions/{sessionId:guid}")] HttpRequest req, string courseId, Guid sessionId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var session = await _attendanceService.GetSession(sessionId);
        return new OkObjectResult(session);
    }
    
    [Function( $"{nameof(SessionsController)}-{nameof(EditAttendance)}")]
    public async Task<IActionResult> EditAttendance([HttpTrigger(AuthorizationLevel.User, "put", Route="courses/{courseId:string}/sessions/{sessionId:guid}/attendance")] HttpRequest req, string courseId, Guid sessionId, [FromBody] UpdateAttendanceContract contract)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        await _attendanceService.SetAttendance(sessionId, contract.UserId, contract.Kind);
        
        return new NoContentResult();
    }
    
    [Function( $"{nameof(SessionsController)}-{nameof(DeleteSession)}")]
    public async Task<IActionResult> DeleteSession([HttpTrigger(AuthorizationLevel.User, "delete", Route="courses/{courseId:string}/sessions/{sessionId:guid}")] HttpRequest req, string courseId, Guid sessionId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        
        await _attendanceService.DeleteSession(sessionId);
        
        return new NoContentResult();
    }
    
    // [Function( $"{nameof(SessionsController)}-{nameof(GetCourseAttendanceReport)}")]
    // public async Task<IActionResult> GetCourseAttendanceReport(
    //     [HttpTrigger(AuthorizationLevel.User, "get", Route = "courses/{courseId:string}/attendance")] HttpRequest req,
    //     string courseId)
    // {
    //     _logger.LogInformation("C# HTTP trigger function processed a request.");
    //     // Replace with actual logic from AttendanceService
    //     return new OkObjectResult($"Welcome to Azure Functions!");
    // }
    //
    // [Function( $"{nameof(SessionsController)}-{nameof(GetAttendanceReport)}")]
    // public async Task<IActionResult> GetAttendanceReport(
    //     [HttpTrigger(AuthorizationLevel.User, "get",
    //         Route = "courses/{courseId:string}/sessions/{sessionId:guid}/attendance")]
    //     HttpRequest req, string courseId, Guid sessionId)
    // {
    //     _logger.LogInformation("C# HTTP trigger function processed a request.");
    //     // Replace with actual logic from AttendanceService
    //     return new OkObjectResult($"Welcome to Azure Functions!");
    // }
}
