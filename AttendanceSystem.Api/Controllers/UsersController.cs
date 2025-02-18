using AttendanceSystem.Api.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Domain.Services;

namespace AttendanceSystem.Api.Controllers;

public class UsersController
{
    private readonly ILogger<UsersController> _logger;
    private readonly UserService _userService;

    public UsersController(ILogger<UsersController> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [Function( $"{nameof(UsersController)}-{nameof(GetAllUsers)}")]
    public async Task<IActionResult> GetAllUsers([HttpTrigger(AuthorizationLevel.User, "get", Route="users")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var users = await _userService.GetUsers();
        return new OkObjectResult(users);
    }

    [Function( $"{nameof(UsersController)}-{nameof(CreateUser)}")]
    public async Task<IActionResult> CreateUser([HttpTrigger(AuthorizationLevel.User, "post", Route="users")] HttpRequest req, [FromBody] CreateUserContract contract)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        return new OkObjectResult($"Welcome to Azure Functions!");
    }

    [Function($"{nameof(UsersController)}-{nameof(ConfigureUser)}")]
    public async Task<IActionResult> ConfigureUser(
        [HttpTrigger(AuthorizationLevel.User, "put", Route="users/{userId:guid}")] HttpRequest req, Guid userId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        // Replace with actual logic from UserService
        return new OkObjectResult($"Welcome to Azure Functions!");
    }

    [Function( $"{nameof(UsersController)}-{nameof(GetUser)}")]
    public async Task<IActionResult> GetUser([HttpTrigger(AuthorizationLevel.User, "get", Route="users/{userId:guid}")] HttpRequest req, Guid userId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var user = await _userService.GetUser(userId);
        return new OkObjectResult(user);
    }

    [Function( $"{nameof(UsersController)}-{nameof(DeleteUser)}")]
    public async Task<IActionResult> DeleteUser([HttpTrigger(AuthorizationLevel.User, "delete", Route="users/{userId:guid}")] HttpRequest req, Guid userId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        // Replace with actual logic from UserService
        return new OkObjectResult($"Welcome to Azure Functions!");
    }
}
