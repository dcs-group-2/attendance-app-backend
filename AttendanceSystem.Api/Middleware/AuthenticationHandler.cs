using System.Text.Json;
using AttendanceSystem.Domain.Model.Exceptions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AttendanceSystem.Api.Middleware;

public class AuthenticationHandler(ILogger<ExceptionToErrorCodeHandler> logger, JsonWebTokenHandler jwtHandler) : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var requestData = await context.GetHttpRequestDataAsync() ?? throw new Exception("Request data is null");
        
        // If the function has something to do with swagger, let it through
        // Also check the current http path
        if (context.FunctionDefinition.Name.StartsWith("Swagger")
            && requestData.Url.LocalPath.Contains("swagger"))
        {
            await next(context);
        }
        
        // Check the bearer token
        string? bearer = requestData.Headers.TryGetValues("Authorization", out var values) ? values.FirstOrDefault(a => a.StartsWith("Bearer")) : null;

        if (bearer is null)
        {
            throw new UnauthorizedAccessException("Bearer token is missing");
        }
        
        var token = bearer.Split(" ").Last();
        
        // Decompile the JWT Token
        TokenValidationParameters tokenValidationParameters = new()
        {
            
        };

        var validationResult = await jwtHandler.ValidateTokenAsync(token, tokenValidationParameters);
        
        if (!validationResult.IsValid)
        {
            throw new UnauthorizedAccessException("Invalid token");
        }
        
        // Serialize the token and add it to the data
        JsonWebToken jwtToken = jwtHandler.ReadJsonWebToken(token);
        
        context.Items.Add("jwt", jwtToken);
        
        await next(context);
    }
}