using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Api.Controllers;

public class CoursesController
{
    private readonly ILogger<CoursesController> _logger;
    
    public CoursesController(ILogger<CoursesController> logger)
    {
        _logger = logger;
    }
    
    [Function( $"{nameof(CoursesController)}-{nameof(GetAllCourses)}")]
    public IActionResult GetAllCourses([HttpTrigger(AuthorizationLevel.User, "get", Route="courses")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }
    
    [Function( $"{nameof(CoursesController)}-{nameof(CreateNewCourse)}")]
    public IActionResult CreateNewCourse([HttpTrigger(AuthorizationLevel.User, "post", Route="courses")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }
    
    [Function( $"{nameof(CoursesController)}-{nameof(GetCourse)}")]
    public IActionResult GetCourse([HttpTrigger(AuthorizationLevel.User, "get", Route="courses/{courseId:guid}")] HttpRequest req, Guid courseId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }
    
    [Function( $"{nameof(CoursesController)}-{nameof(ConfigureCourse)}")]
    public IActionResult ConfigureCourse([HttpTrigger(AuthorizationLevel.User, "put", Route="courses/{courseId:guid}")] HttpRequest req, Guid courseId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }
    
    [Function( $"{nameof(CoursesController)}-{nameof(DeleteCourse)}")]
    public IActionResult DeleteCourse([HttpTrigger(AuthorizationLevel.User, "delete", Route="courses/{courseId:guid}")] HttpRequest req, Guid courseId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }
    
    [Function( $"{nameof(CoursesController)}-{nameof(EnrollUser)}")]
    public IActionResult EnrollUser([HttpTrigger(AuthorizationLevel.User, "post", Route="courses/{courseId:guid}/participants")] HttpRequest req, Guid courseId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }
}
