using AttendanceSystem.Api.Contracts;
using AttendanceSystem.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Domain.Services;
using AttendanceSystem.Domain.Services.Alterations;

using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

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
    public async Task<IActionResult> GetAllUsers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route="users")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var users = await _userService.GetAllStudents();
        return new OkObjectResult(users);
    }

    [Function( $"{nameof(UsersController)}-{nameof(CreateUser)}")]
    public async Task<IActionResult> CreateUser([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route="users")] HttpRequest req, [FromBody] CreateUserContract contract)
    {
        User user = contract.Type switch 
        {
            UserType.Student => await _userService.CreateStudent(contract.Id, contract.Name, contract.Email),
            UserType.Teacher => await _userService.CreateTeacher(contract.Id, contract.Name, contract.Email),
            UserType.Administrator => await _userService.CreateAdministrator(contract.Id, contract.Name, contract.Email),
            _ => throw new NotSupportedException("The user type is not supported.")
        };

        return new OkObjectResult(user);
    }

    [Function($"{nameof(UsersController)}-{nameof(ConfigureUser)}")]
    public async Task<IActionResult> ConfigureUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route="users/{userId}")] HttpRequest req, string userId, UserAlteration alteration)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        
        // Configure the user
        User user = await _userService.EditUser(userId, alteration);
        
        return new OkObjectResult(user);
    }

    [Function( $"{nameof(UsersController)}-{nameof(GetUser)}")]
    public async Task<IActionResult> GetUser([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route="users/{userId}")] HttpRequest req, string userId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var user = await _userService.GetUser(userId);
        return new OkObjectResult(user);
    }

    [Function( $"{nameof(UsersController)}-{nameof(DeleteUser)}")]
    public async Task<IActionResult> DeleteUser([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route="users/{userId}")] HttpRequest req, string userId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        await _userService.DeleteUser(userId);
        
        return new NoContentResult();
    }
}
