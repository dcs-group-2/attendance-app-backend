using AzureFunctions.Extensions.Swashbuckle;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using AzureFunctions.Extensions.Swashbuckle.SwashBuckle;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Api.Controllers;

/// <summary>
/// Represents the controller that serves the swagger documents
/// </summary>
/// <param name="logger"></param>
/// <param name="swashBuckleClient"></param>
public class SwaggerController(ILogger<SwaggerController> logger, ISwashBuckleClient swashBuckleClient)
{
    private readonly ILogger<SwaggerController> _logger = logger;

    [SwaggerIgnore]
    [Function("Swagger-Json")]
    public async Task<HttpResponseData> SwaggerJson(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/json")]
        HttpRequestData  req)
    {
        return await swashBuckleClient.CreateSwaggerJsonDocumentResponse(req);
    }

    [SwaggerIgnore]
    [Function("Swagger-Ui")]
    public async Task<HttpResponseData> SwaggerUi(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/ui")]
        HttpRequestData req)
    {
        return await swashBuckleClient.CreateSwaggerUIResponse(req, "swagger/json");
    }

    /// <summary>
    /// This is only needed for OAuth2 client. This redirecting document is normally served
    /// as a static content. Functions don't provide this out of the box, so we serve it here.
    /// Don't forget to set OAuth2RedirectPath configuration option to reflect this route.
    /// </summary>
    /// <param name="req">The request given by the azure functions</param>
    /// <returns>An OAuth2 Redirect Response</returns>
    [SwaggerIgnore]
    [Function("Swagger-OAuth2Redirect")]
    public async Task<HttpResponseData> SwaggerOAuth2Redirect(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/oauth2-redirect")]
        HttpRequestData req)
    {
        return await swashBuckleClient.CreateSwaggerOAuth2RedirectResponse(req);
    }

}