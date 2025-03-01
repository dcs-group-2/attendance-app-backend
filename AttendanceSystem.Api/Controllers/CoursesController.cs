﻿using AttendanceSystem.Api.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Domain.Services;
using AttendanceSystem.Domain.Services.Alterations;

using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

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
    public async Task<IActionResult> GetAllCourses([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route="courses")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var courses = await _courseService.GetAllCourses();
        return new OkObjectResult(courses);
    }

    [Function( $"{nameof(CoursesController)}-{nameof(CreateNewCourse)}")]
    public async Task<IActionResult> CreateNewCourse([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route="courses")] HttpRequest req, [FromBody] CreateCourseContract contract)
    {
        var course = await _courseService.CreateNewCourse(contract.Id, contract.Name, contract.DepartmentId, contract.TeacherIds);
        return new OkObjectResult(course);
    }

    [Function( $"{nameof(CoursesController)}-{nameof(GetCourse)}")]
    public async Task<IActionResult> GetCourse([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route="courses/{courseId}")] HttpRequest req, string courseId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var course = await _courseService.GetCourse(courseId);
        return new OkObjectResult(course);
    }

    [Function( $"{nameof(CoursesController)}-{nameof(ConfigureCourse)}")]
    public async Task<IActionResult> ConfigureCourse([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route="courses/{courseId}")] HttpRequest req, string courseId, [FromBody] CourseAlteration alteration)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var course = await _courseService.ConfigureCourse(courseId, alteration);
        return new OkObjectResult(course);
    }

    [Function( $"{nameof(CoursesController)}-{nameof(DeleteCourse)}")]
    public async Task<IActionResult> DeleteCourse([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route="courses/{courseId}")] HttpRequest req, string courseId)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        await _courseService.DeleteCourse(courseId);
        return new NoContentResult();
    }

    [Function( $"{nameof(CoursesController)}-{nameof(EnrollUser)}")]
    public async Task<IActionResult> EnrollUser([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route="courses/{courseId}/participants")] HttpRequest req, string courseId, [FromBody] EnrollUserContract contract)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        await _courseService.EnrollUser(courseId, contract.UserId);
        return new NoContentResult();
    }
}
