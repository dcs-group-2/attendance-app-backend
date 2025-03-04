using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AttendanceSystem.Api;

public class AuthenticationService(ILogger<AuthenticationService> logger, JsonWebTokenHandler jwtHandler)
{
    public async Task<bool> IsAuthenticated(TokenValidationResult jwt, IEnumerable<string> requiredRoles)
    {
        if (!jwt.Claims.TryGetValue("roles", out object? rolesClaim)) return false;

        // Get the roles from the claims
        var roles = rolesClaim as List<object>;
        if (roles is null)
        {
            // It could be that there is just one role
            var role = rolesClaim as string;

            if (role is not null && requiredRoles.Contains(role)) return true;

            return false;
        }


        var isAuthenticated = roles.Any(requiredRoles.Contains);

        return jwt.IsValid && isAuthenticated;
    }
}
