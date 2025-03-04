using AttendanceSystem.Api.Contracts;
using AttendanceSystem.Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Domain.Services;
using AttendanceSystem.Domain.Services.Alterations;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using static AttendanceSystem.Api.Roles;

using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace AttendanceSystem.Api.Controllers;

public class UsersController : BaseController
{
    private readonly ILogger<UsersController> _logger;
    private readonly UserService _userService;

    public UsersController(ILogger<UsersController> logger, UserService userService, AuthenticationService authenticationService) : base(authenticationService)
    {
        _logger = logger;
        _userService = userService;
    }

    [Function( $"{nameof(UsersController)}-{nameof(GetAllUsers)}")]
    public async Task<IActionResult> GetAllUsers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route="users")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx)
    {
        // Assert user is authenticated
        await AssertAuthentication(ctx, [Admin]);

        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var users = await _userService.GetAllStudents();
        return new OkObjectResult(users);
    }

    [Function( $"{nameof(UsersController)}-{nameof(CreateUser)}")]
    public async Task<IActionResult> CreateUser([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route="users")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx, [FromBody] CreateUserContract contract)
    {
        await AssertAuthentication(ctx, [Admin]);

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
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route="users/{userId}")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx, string userId, UserAlteration alteration)
    {
        await AssertAuthentication(ctx, [Admin]);

        _logger.LogInformation("C# HTTP trigger function processed a request.");

        // Configure the user
        User user = await _userService.EditUser(userId, alteration);

        return new OkObjectResult(user);
    }

    [Function( $"{nameof(UsersController)}-{nameof(GetUser)}")]
    public async Task<IActionResult> GetUser([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route="users/{userId}")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx, string userId)
    {
        await AssertAuthentication(ctx, AllowElevated);

        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var user = await _userService.GetUser(userId);
        return new OkObjectResult(user);
    }

    [Function( $"{nameof(UsersController)}-{nameof(DeleteUser)}")]
    public async Task<IActionResult> DeleteUser([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route="users/{userId}")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx, string userId)
    {
        await AssertAuthentication(ctx, [Admin]);

        _logger.LogInformation("C# HTTP trigger function processed a request.");

        await _userService.DeleteUser(userId);

        return new NoContentResult();
    }
}
