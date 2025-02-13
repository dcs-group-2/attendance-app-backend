using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Api.Controllers;

public class UsersController
{
    private readonly ILogger<UsersController> _logger;
    
    public UsersController(ILogger<UsersController> logger)
    {
        _logger = logger;
    }
    
    [Function( $"{nameof(UsersController)}-{nameof(GetAllUsers)}")]
    public IActionResult GetAllUsers([HttpTrigger(AuthorizationLevel.User, "get", Route="users")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }
    
    [Function( $"{nameof(UsersController)}-{nameof(CreateUser)}")]
    public IActionResult CreateUser([HttpTrigger(AuthorizationLevel.User, "post", Route="users")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }

    [Function($"{nameof(UsersController)}-{nameof(ConfigureUser)}")]
    public IActionResult ConfigureUser(
        [HttpTrigger(AuthorizationLevel.User, "put", Route="users/{userId:guid}")] HttpRequest req, Guid userId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }

    [Function( $"{nameof(UsersController)}-{nameof(GetUser)}")]
    public IActionResult GetUser([HttpTrigger(AuthorizationLevel.User, "get", Route="users/{userId:guid}")] HttpRequest req, Guid userId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }
    
    [Function( $"{nameof(UsersController)}-{nameof(DeleteUser)}")]
    public IActionResult DeleteUser([HttpTrigger(AuthorizationLevel.User, "delete", Route="users/{userId:guid}")] HttpRequest req, Guid userId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!");
    }

    
    
    // [Function( $"{nameof(UsersController)}-{nameof(ConfigureCourse)}")]
    // public IActionResult ConfigureCourse([HttpTrigger(AuthorizationLevel.User, "put", Route="courses/{courseId:guid}")] HttpRequest req, Guid courseId)
    // {
    //     _logger.LogInformation("C# HTTP trigger function processed a request.");
    //     return new OkObjectResult($"Welcome to Azure Functions!");
    // }
    //
    // [Function( $"{nameof(UsersController)}-{nameof(DeleteCourse)}")]
    // public IActionResult DeleteCourse([HttpTrigger(AuthorizationLevel.User, "delete", Route="courses/{courseId:guid}")] HttpRequest req, Guid courseId)
    // {
    //     _logger.LogInformation("C# HTTP trigger function processed a request.");
    //     return new OkObjectResult($"Welcome to Azure Functions!");
    // }
    //
    // [Function( $"{nameof(UsersController)}-{nameof(EnrollUser)}")]
    // public IActionResult EnrollUser([HttpTrigger(AuthorizationLevel.User, "post", Route="courses/{courseId:guid}/participants")] HttpRequest req, Guid courseId)
    // {
    //     _logger.LogInformation("C# HTTP trigger function processed a request.");
    //     return new OkObjectResult($"Welcome to Azure Functions!");
    // }
}
