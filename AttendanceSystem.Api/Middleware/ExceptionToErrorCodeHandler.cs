using System.Text.Json;
using AttendanceSystem.Domain.Model.Exceptions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace AttendanceSystem.Api.Middleware;

public class ExceptionToErrorCodeHandler : IFunctionsWorkerMiddleware
{
    private readonly ILogger<ExceptionToErrorCodeHandler> _logger;

    public ExceptionToErrorCodeHandler(ILogger<ExceptionToErrorCodeHandler> logger)
    {
        _logger = logger;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
            Console.WriteLine("No exception occurred");
        }
        catch (Exception ex)
        {
            var request = await context.GetHttpRequestDataAsync();
            var response = request!.CreateResponse();
            string message = "Something went wrong.";
            
            // Get the error code and message for the exception
            switch (ex)
            {
                case NotImplementedException:
                    response.StatusCode = System.Net.HttpStatusCode.NotImplemented;
                    message = "This feature is not implemented yet.";
                    break;
                case UnauthorizedAccessException:
                    response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    message = "You are not authorized to perform this action.";
                    break;
                case InvalidOperationException { Source: "Microsoft.Azure.Functions.Worker.Extensions.Http.AspNetCore" }:
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    message = "The request is not formatted properly.";
                    break;
                case EntityNotFoundException entityNotFoundException:
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    message = entityNotFoundException.Message;
                    break;
                default:
                    _logger.LogError(ex, "An unhandled exception occurred.");
                    response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                    message = "An unhandled exception occurred. Please try again later";
                    break;
            }
            
            var errorMessage = new { Status = response.StatusCode, Message = message };
            string responseBody = JsonSerializer.Serialize(errorMessage);
            response.Headers.Add("Content-Type", "application/json");

            await response.WriteStringAsync(responseBody);
            context.GetInvocationResult().Value = response;
        }
    }
}