using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AttendanceSystem.Api;

public class AuthenticationService(ILogger<AuthenticationService> logger, JsonWebTokenHandler jwtHandler)
{
    public List<string> GetRoles(TokenValidationResult jwt)
    {
        if (!jwt.Claims.TryGetValue("roles", out var rolesClaim)) return [];

        return rolesClaim switch
        {
            string role => [role],
            List<object> roles => roles.Cast<string>().ToList(),
            _ => [],
        };
    }
    
    public bool IsAuthenticated(TokenValidationResult jwt, IEnumerable<string> requiredRoles)
    {
        if (!jwt.Claims.TryGetValue("roles", out object? rolesClaim)) return false;

        return jwt.IsValid && rolesClaim switch
        {
            string role => requiredRoles.Contains(role),
            List<object> roles => roles.Any(requiredRoles.Contains),
            _ => false,
        };
    }
}
