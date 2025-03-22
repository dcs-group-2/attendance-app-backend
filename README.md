# Attendance App Backend

The Attendance App Backend is a robust, multi-layered application built with .NET. It handles user management, course scheduling, attendance recording, and real-time feed updates.

## Features

- **User Management:** Easily create and manage user profiles.
- **Course & Session Management:** Schedule courses and sessions, and record attendance.
- **Feeds:** Feeds combine three or more separate API requests for an improved API usability.
- **Domain-Driven Design:** Clear separation of concerns with dedicated projects for API, data access, domain models, and services.
- **Robust Error Handling:** Custom middleware ensures secure authentication and graceful error management.
- **CI/CD Pipelines:** Automated workflows for building, testing, and deploying the application.
- **Documentation:** Built-in Swagger for API documentation.

## Repository Structure

The project is organized into several key directories:

```
attendance-app-backend-main/
├── .github/                         # Workflows for analysis and building
├── AttendanceSystem.Api/            # API layer with controllers, middleware, and services
├── AttendanceSystem.Data/           # Data access layer using Entity Framework Core
├── AttendanceSystem.Domain.Model/   # Domain models representing the business logic
├── AttendanceSystem.Domain.Services/# Business logic and service layer
├── AttendanceSystem.sln             # Visual Studio solution file
├── docfx.json                       # Documentation configuration for DocFX
└── main.bicep                       # Bicep file for Azure deployments
```

### Notable Directories & Files

- **.github:** Contains GitHub Actions workflows for dependency updates, code analysis, documentation builds, and deployments.
- **AttendanceSystem.Api:** The core of the backend. Contains controllers, contracts, and middleware that form the RESTful API.
- **AttendanceSystem.Data:** Manages the data context and migrations for the database.
- **AttendanceSystem.Domain.Model:** Defines all the core business entities and exceptions.
- **AttendanceSystem.Domain.Services:** Houses business logic and services, including data manipulation and domain-specific tools.
- **docfx.json & main.bicep:** Support documentation generation and cloud deployment, respectively.

## Getting Started

### Prerequisites

- [.NET 9 SDK or later](https://dotnet.microsoft.com/download)
- [SQL Server or another compatible database](https://www.microsoft.com/en-us/sql-server)
- A code editor such as [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio](https://visualstudio.microsoft.com/)

### Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/dcs-group-2/attendance-app-backend-main.git
   cd attendance-app-backend-main
   ```

2. **Restore dependencies:**

   ```bash
   dotnet restore
   ```

3. **Configure your environment:**

   Update the connection strings and other necessary settings in your `local.settings.json` or via environment variables.

### Running the Application

To start the API locally:

```bash
cd AttendanceSystem.Api
dotnet run
```

The API should start on the default ports (commonly `http://localhost:5000` or `https://localhost:5001`). Swagger documentation will be available at `http://localhost:5000/api/swagger/ui`.

## API Endpoints

The application follows RESTful principles. Primary endpoints include:

- **Users:** `/api/users`
- **Courses:** `/api/courses`
- **Sessions:** `/api/courses/{course-id}/sessions`
- **Feeds:** `/api/feeds`

Explore all endpoints interactively through the integrated Swagger UI.

## CI/CD and Workflows

The repository integrates several GitHub Actions workflows to streamline the development process:

- **Dependency Management:** Automated dependency updates via Dependabot.
- **Static Code Analysis:** CodeQL and SonarQube workflows to maintain high code quality.
- **Documentation:** Automated builds for up-to-date API documentation.
- **Deployments:** Bicep-based workflows to deploy to Azure.

## Contributing

Contributions to enhance the project are welcome! Please:

1. Fork the repository.
2. Create a new branch for your feature or fix.
3. Commit your changes with clear and descriptive messages.
4. Open a pull request for review.

We highly encourage developers to contribute to the projbect. Please open an issue first to discuss the implementation details and ensure that no redundant work is being done.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Contact

For support or questions, please submit a bug report!
