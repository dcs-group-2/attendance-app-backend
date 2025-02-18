namespace AttendanceSystem.Domain.Model;

public class User
{
    /// <summary>
    /// Gets or sets the internal ID managed by the external identity provider.
    /// </summary>
    /// <remarks>
    /// We want to store this because otherwise, we cannot identify the user based on the
    /// information provided by the identity provider.
    /// </remarks>
    public string Id { get; init; }

    public string Name { get; set; }

    public string Email { get; set; }

    public List<Course> Courses { get; set; }
}
