using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Api.Controllers;

public class TestController
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }

    [Function("Test123")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.User, "get", "post", Route="Hello/{name:alpha}")] HttpRequest req, string name)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult($"Welcome to Azure Functions!, {name}");
    }

}