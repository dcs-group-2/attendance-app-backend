using AttendanceSystem.Api.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Domain.Services;

namespace AttendanceSystem.Api.Controllers;

public class CoursesController
{
    private readonly ILogger<CoursesController> _logger;
    private readonly CourseService _courseService;

    public CoursesController(ILogger<CoursesController> logger, CourseService courseService)
    {
        _logger = logger;
        _courseService = courseService;
    }

    [Function( $"{nameof(CoursesController)}-{nameof(GetAllCourses)}")]
    public async Task<IActionResult> GetAllCourses([HttpTrigger(AuthorizationLevel.User, "get", Route="courses")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var courses = await _courseService.GetAllCourses();
        return new OkObjectResult(courses);
    }

    [Function( $"{nameof(CoursesController)}-{nameof(CreateNewCourse)}")]
    public async Task<IActionResult> CreateNewCourse([HttpTrigger(AuthorizationLevel.User, "post", Route="courses")] HttpRequest req, [FromBody] CreateCourseContract contract)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var course = await _courseService.CreateNewCourse(contract);
        return new OkObjectResult(course);
    }

    [Function( $"{nameof(CoursesController)}-{nameof(GetCourse)}")]
    public async Task<IActionResult> GetCourse([HttpTrigger(AuthorizationLevel.User, "get", Route="courses/{courseId:guid}")] HttpRequest req, Guid courseId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var course = await _courseService.GetCourse(courseId);
        return new OkObjectResult(course);
    }

    [Function( $"{nameof(CoursesController)}-{nameof(ConfigureCourse)}")]
    public async Task<IActionResult> ConfigureCourse([HttpTrigger(AuthorizationLevel.User, "put", Route="courses/{courseId:guid}")] HttpRequest req, Guid courseId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var course = await _courseService.ConfigureCourse(req, courseId);
        return new OkObjectResult(course);
    }

    [Function( $"{nameof(CoursesController)}-{nameof(DeleteCourse)}")]
    public async Task<IActionResult> DeleteCourse([HttpTrigger(AuthorizationLevel.User, "delete", Route="courses/{courseId:guid}")] HttpRequest req, Guid courseId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        await _courseService.DeleteCourse(courseId);
        return new OkObjectResult($"Course {courseId} deleted.");
    }

    [Function( $"{nameof(CoursesController)}-{nameof(EnrollUser)}")]
    public async Task<IActionResult> EnrollUser([HttpTrigger(AuthorizationLevel.User, "post", Route="courses/{courseId:guid}/participants")] HttpRequest req, Guid courseId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var user = await _courseService.EnrollUser(req, courseId);
        return new OkObjectResult(user);
    }
}
