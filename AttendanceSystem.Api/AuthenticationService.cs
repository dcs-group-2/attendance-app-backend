using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AttendanceSystem.Api;

public class AuthenticationService(ILogger<AuthenticationService> logger, JsonWebTokenHandler jwtHandler)
{
    public async Task<bool> IsAuthenticated(TokenValidationResult jwt, IEnumerable<string> requiredRoles)
    {
        // Get the roles from the claims
        var roles = jwt.Claims["roles"] as IEnumerable<string>;
        if (roles == null) return false;
        
        var isAuthenticated = roles.Any(requiredRoles.Contains);
        
        return jwt.IsValid && isAuthenticated;
    }
}