using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AttendanceSystem.Api;

public class AuthenticationService(ILogger<AuthenticationService> logger, JsonWebTokenHandler jwtHandler)
{
    public async Task<bool> IsAuthenticated(TokenValidationResult jwt, IEnumerable<string> requiredRoles)
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
