using System.Text.Json;
using AttendanceSystem.Domain.Model.Exceptions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace AttendanceSystem.Api.Middleware;

public class AuthenticationHandler(ILogger<AuthenticationHandler> logger, JsonWebTokenHandler jwtHandler)
    : IFunctionsWorkerMiddleware
{
    private readonly ILogger<AuthenticationHandler> _logger = logger;
    private readonly string _tenantId = Environment.GetEnvironmentVariable("TenantId") ?? throw new Exception("Tenant ID is missing");
    private OpenIdConnectConfiguration? _openIdConfig = null!;

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var requestData = await context.GetHttpRequestDataAsync() ?? throw new Exception("Request data is null");
        
        // If the function has something to do with swagger, let it through
        // Also check the current http path
        if (context.FunctionDefinition.Name.StartsWith("Swagger")
            && requestData.Url.LocalPath.Contains("swagger"))
        {
            await next(context);
            return;
        }
        
        // Check the bearer token
        string? bearer = requestData.Headers.TryGetValues("Authorization", out var values) 
            ? values.FirstOrDefault(a => a.StartsWith("Bearer")) 
            : null;

        if (bearer is null)
        {
            throw new UnauthorizedAccessException("Bearer token is missing");
        }
        
        var token = bearer.Split(" ").Last();


        if (_openIdConfig is null)
        {
            var discoveryEndpoint = $"https://login.microsoftonline.com/{_tenantId}/v2.0/.well-known/openid-configuration";
            
            // Retrieve the signing keys
            var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                discoveryEndpoint,
                new OpenIdConnectConfigurationRetriever());
            _openIdConfig = await configManager.GetConfigurationAsync(CancellationToken.None);
        }
        
        // Configure token validation parameters
        TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _openIdConfig.Issuer,
            ValidateAudience = true,
            ValidAudiences = ["api://uva-devops-attendance-app", "1d5d790f-f1c2-41bc-80df-e57e3642b219"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = _openIdConfig.SigningKeys,
        };

        var validationResult = await jwtHandler.ValidateTokenAsync(token, tokenValidationParameters);
        
        if (!validationResult.IsValid)
        {
            throw new UnauthorizedAccessException("Invalid token");
        }
        
        // Serialize the token and add it to the data
        JsonWebToken jwtToken = jwtHandler.ReadJsonWebToken(token);
        
        context.Items.Add("jwt", jwtToken);
        context.Items.Add("validationResult", validationResult);
        context.Items.Add("roles", validationResult.Claims["roles"]);
        
        await next(context);
    }
}