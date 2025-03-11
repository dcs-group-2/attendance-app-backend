using AttendanceSystem.Api.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Domain.Services;
using AttendanceSystem.Domain.Services.Alterations;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;
using static AttendanceSystem.Api.Roles;

namespace AttendanceSystem.Api.Controllers;

public class CoursesController : BaseController
{
    private readonly ILogger<CoursesController> _logger;
    private readonly CourseService _courseService;
    private readonly AuthenticationService _authenticationService;

    public CoursesController(ILogger<CoursesController> logger, CourseService courseService, AuthenticationService authenticationService, UserService userService)
        : base(authenticationService, userService)
    {
        _logger = logger;
        _courseService = courseService;
        _authenticationService = authenticationService;
    }

    [Function($"{nameof(CoursesController)}-{nameof(GetAllCourses)}")]
    public async Task<IActionResult> GetAllCourses([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "courses")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx)
    {
        // Authorize
        await AssertAuthentication(ctx, AllowAll);

        _logger.LogInformation("C# HTTP trigger function processed a request.");
        if (GetUserRoles(ctx).Contains(Admin))
        {
            var allCourses = await _courseService.GetAllCourses();
            return new OkObjectResult(allCourses);
        }
        
        var courses = await _courseService.GetAllCourses(GetUserId(ctx));
        return new OkObjectResult(courses);
    }

    [Function($"{nameof(CoursesController)}-{nameof(CreateNewCourse)}")]
    public async Task<IActionResult> CreateNewCourse([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "courses")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx, [FromBody] CreateCourseContract contract)
    {
        // Authorize
        await AssertAuthentication(ctx, [Admin]);

        var course = await _courseService.CreateNewCourse(contract.Id, contract.Name, contract.DepartmentId, contract.TeacherIds);
        return new OkObjectResult(course);
    }

    [Function($"{nameof(CoursesController)}-{nameof(GetCourse)}")]
    public async Task<IActionResult> GetCourse([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "courses/{courseId}")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx, string courseId)
    {
        // Authorize
        await AssertAuthentication(ctx, AllowAll);

        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var course = await _courseService.GetCourse(courseId);
        return new OkObjectResult(course);
    }

    [Function($"{nameof(CoursesController)}-{nameof(ConfigureCourse)}")]
    public async Task<IActionResult> ConfigureCourse([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "courses/{courseId}")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx, string courseId, [FromBody] CourseAlteration alteration)
    {
        // Authorize
        await AssertAuthentication(ctx, [Admin]);

        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var course = await _courseService.ConfigureCourse(courseId, alteration);
        return new OkObjectResult(course);
    }

    [Function($"{nameof(CoursesController)}-{nameof(DeleteCourse)}")]
    public async Task<IActionResult> DeleteCourse([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "courses/{courseId}")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx, string courseId)
    {
        // Authorize
        await AssertAuthentication(ctx, [Admin]);

        _logger.LogInformation("C# HTTP trigger function processed a request.");
        await _courseService.DeleteCourse(courseId);
        return new NoContentResult();
    }

    [Function($"{nameof(CoursesController)}-{nameof(EnrollUser)}")]
    public async Task<IActionResult> EnrollUser([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "courses/{courseId}/participants")] HttpRequest req, [SwaggerIgnore] FunctionContext ctx, string courseId, [FromBody] EnrollUserContract contract)
    {
        // Authorize
        await AssertAuthentication(ctx, AllowElevated);

        if (!GetUserRoles(ctx).Contains(Roles.Admin) && !await _courseService.UserCanAccessCourse(courseId, GetUserId(ctx)))
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this course.");
        }

        _logger.LogInformation("C# HTTP trigger function processed a request.");
        await _courseService.EnrollUser(courseId, contract.UserId);
        return new NoContentResult();
    }
}
