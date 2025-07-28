# Fredrik's Orders Dashboard

A modern ASP.NET Blazor Server application for managing and visualizing order data.

## Overview

Fredrik's Orders Dashboard is a web-based application built with ASP.NET Core Blazor Server that provides a clean, intuitive interface for order management. The application features a streamlined dashboard design focused on displaying and managing order information.

## Features

- **Clean Dashboard Interface**: Streamlined view without navigation clutter, focusing on order data
- **Blazor Server Technology**: Leverages the latest .NET 8 framework for optimal performance
- **Responsive Design**: Modern UI that adapts to different screen sizes
- **Unit Testing**: Comprehensive test coverage using xUnit framework

## Technology Stack

- **Framework**: .NET 8 (LTS)
- **Web Framework**: ASP.NET Core Blazor Server
- **Testing**: xUnit
- **CI/CD**: GitHub Actions
- **Target Platform**: Cross-platform (Windows, Linux, macOS)

## Project Structure

```
├── src/
│   └── OrdersDashboard.Web/          # Main web application
│       ├── Components/               # Blazor components
│       ├── wwwroot/                 # Static files
│       └── Program.cs               # Application entry point
├── tests/
│   └── OrdersDashboard.Tests/       # Unit tests
├── .github/
│   └── workflows/                   # CI/CD pipeline
└── OrdersDashboard.sln             # Solution file
```

## Getting Started

### Prerequisites

- .NET 8 SDK or later
- Any modern IDE (Visual Studio, VS Code, JetBrains Rider)

### Building the Application

```bash
# Restore dependencies
dotnet restore

# Build the solution
dotnet build --configuration Release

# Run tests
dotnet test

# Run the application
cd src/OrdersDashboard.Web
dotnet run
```

The application will be available at `https://localhost:5001` (HTTPS) or `http://localhost:5000` (HTTP).

### Development

The application uses Blazor Server-side rendering for optimal performance and SEO. The main dashboard is accessible at the root URL and provides a focused view for order management without sidebar navigation.

## Testing

Unit tests are located in the `tests/OrdersDashboard.Tests` project and can be run using:

```bash
dotnet test --verbosity normal
```

## CI/CD Pipeline

The project includes GitHub Actions workflows that:
- **CI Workflow**: Builds the application on every push and pull request, runs all unit tests, reports test results as PR comments, and ensures code quality and reliability
- **Teams Notification**: Automatically posts pull request information to Microsoft Teams channel including author, description, files changed, and direct link to the PR

## Contributing

This application is designed as a focused order management dashboard. When contributing:
1. Maintain the clean, navigation-free dashboard design
2. Add appropriate unit tests for new features
3. Follow existing code conventions and patterns
4. Ensure all tests pass before submitting changes

## License

This project is part of a demonstration application for order management workflows.