using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AttendanceSystem.Api;

public class AuthenticationService(Logger<AuthenticationService> logger, JsonWebTokenHandler jwtHandler)
{
    public async Task<bool> IsAuthenticated(JsonWebToken jwt, IEnumerable<string> requiredRoles)
    {
        var validationResult = await jwtHandler.ValidateTokenAsync(jwt, new TokenValidationParameters());
        
        return validationResult.IsValid;
    }
}