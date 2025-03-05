using System.Collections;
using AttendanceSystem.Domain.Model;
using AttendanceSystem.Domain.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.IdentityModel.Tokens;

namespace AttendanceSystem.Api.Controllers;

public class BaseController
{
    private readonly AuthenticationService _authenticationService;
    private readonly UserService _userService;
    
    public BaseController(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    protected Task<User> GetUser(FunctionContext context)
    {
        // Get the JWT token
        TokenValidationResult? token = context.Items["validationResult"] as TokenValidationResult;
        
        // Get the openID value
        if (token.Claims.TryGetValue("oid", out var oid) || oid is not string openId)
        {
            throw new ArgumentException("OpenID was not found in JWT claims.");
        }
        
        return _userService.GetUser(openId);
    }

    protected async Task AssertAuthentication(FunctionContext context, IEnumerable<string> allowedRoles)
    {
        // Get the JWT token
        TokenValidationResult? token = context.Items["validationResult"] as TokenValidationResult;
        if (token is null)
        {
            throw new ArgumentException("Token was not found.");
        }

        if (!await _authenticationService.IsAuthenticated(token, allowedRoles))
        {
            throw new UnauthorizedAccessException();
        }
    }
}