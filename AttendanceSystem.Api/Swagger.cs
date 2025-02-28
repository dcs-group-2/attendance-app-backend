using System.Reflection;
using AttendanceSystem.Api.Controllers;
using AzureFunctions.Extensions.Swashbuckle;
using AzureFunctions.Extensions.Swashbuckle.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace AttendanceSystem.Api;

public static class SwaggerSetup
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwashBuckle(opts =>
        {
            opts.RoutePrefix = "api";
            opts.SpecVersion = OpenApiSpecVersion.OpenApi3_0;
            opts.PrependOperationWithRoutePrefix = true;
            opts.XmlPath = $"{AppContext.BaseDirectory}/AttendanceSystem.Api.xml";
            opts.Documents =
            [
                new SwaggerDocument
                {
                    Name = "v1",
                    Title = "Swagger document",
                    Description = "Swagger test document",
                    Version = "v2"
                }
            ];
            opts.Title = "UvA Attendance System API";
            opts.ConfigureSwaggerGen = x =>
            {
                // Get the tenant id
                var tenantId = Environment.GetEnvironmentVariable("TenantId");
                if (string.IsNullOrEmpty(tenantId)) throw new InvalidOperationException("TenantId is not set.");
                
                //oauth2
                x.AddSecurityDefinition("EntraID",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new()
                        {
                            Implicit = new()
                            {
                                AuthorizationUrl =
                                    new Uri($"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/authorize"),
                                TokenUrl = new Uri($"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token"),
                                Scopes = new Dictionary<string, string>
                                {
                                    { "openid", "Access OpenID" },
                                    { "email", "Access email" },
                                    { "api://uva-devops-attendance-app/Admin.Admin", "Access admin operations" },
                                }
                            }
                        }
                    });
                
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                        },
                        []
                    }
                });
            };

            // set up your client ID if your API is protected
            opts.ClientId = Environment.GetEnvironmentVariable("ClientId")!;
            opts.OAuth2RedirectPath = "http://localhost:7017/api/swagger/oauth2-redirect";
        });
        
        return services;
    }
    
}