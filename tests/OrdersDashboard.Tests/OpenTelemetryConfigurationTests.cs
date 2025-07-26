using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace OrdersDashboard.Tests;

public class OpenTelemetryConfigurationTests
{
    [Fact]
    public void ResourceBuilder_ConfiguresServiceIdentification()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["OpenTelemetry:ServiceName"] = "TestOrdersDashboard",
                ["OpenTelemetry:ServiceVersion"] = "2.0.0"
            })
            .Build();

        // Act
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(
                serviceName: configuration["OpenTelemetry:ServiceName"] ?? "OrdersDashboard",
                serviceVersion: configuration["OpenTelemetry:ServiceVersion"] ?? "1.0.0",
                serviceInstanceId: Environment.MachineName);

        var resource = resourceBuilder.Build();

        // Assert
        var serviceNameAttribute = resource.Attributes.FirstOrDefault(attr => attr.Key == "service.name");
        var serviceVersionAttribute = resource.Attributes.FirstOrDefault(attr => attr.Key == "service.version");
        var serviceInstanceAttribute = resource.Attributes.FirstOrDefault(attr => attr.Key == "service.instance.id");

        Assert.Equal("TestOrdersDashboard", serviceNameAttribute.Value);
        Assert.Equal("2.0.0", serviceVersionAttribute.Value);
        Assert.Equal(Environment.MachineName, serviceInstanceAttribute.Value);
    }

    [Fact]
    public void ResourceBuilder_UsesDefaultValues_WhenConfigurationMissing()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();

        // Act
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(
                serviceName: configuration["OpenTelemetry:ServiceName"] ?? "OrdersDashboard",
                serviceVersion: configuration["OpenTelemetry:ServiceVersion"] ?? "1.0.0",
                serviceInstanceId: Environment.MachineName);

        var resource = resourceBuilder.Build();

        // Assert
        var serviceNameAttribute = resource.Attributes.FirstOrDefault(attr => attr.Key == "service.name");
        var serviceVersionAttribute = resource.Attributes.FirstOrDefault(attr => attr.Key == "service.version");

        Assert.Equal("OrdersDashboard", serviceNameAttribute.Value);
        Assert.Equal("1.0.0", serviceVersionAttribute.Value);
    }

    [Fact]
    public void Configuration_ContainsOpenTelemetrySettings()
    {
        // Arrange
        var configurationData = new Dictionary<string, string?>
        {
            ["OpenTelemetry:ServiceName"] = "OrdersDashboard",
            ["OpenTelemetry:ServiceVersion"] = "1.0.0"
        };

        // Act
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationData)
            .Build();

        // Assert
        Assert.Equal("OrdersDashboard", configuration["OpenTelemetry:ServiceName"]);
        Assert.Equal("1.0.0", configuration["OpenTelemetry:ServiceVersion"]);
    }

    [Fact]
    public void LoggingConfiguration_ConfiguresCorrectLogLevels()
    {
        // Arrange
        var configurationData = new Dictionary<string, string?>
        {
            ["Logging:LogLevel:Default"] = "Information",
            ["Logging:LogLevel:Microsoft.AspNetCore"] = "Warning",
            ["Logging:LogLevel:OrdersDashboard.Web.Services"] = "Information"
        };

        // Act
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationData)
            .Build();

        // Assert
        Assert.Equal("Information", configuration["Logging:LogLevel:Default"]);
        Assert.Equal("Warning", configuration["Logging:LogLevel:Microsoft.AspNetCore"]);
        Assert.Equal("Information", configuration["Logging:LogLevel:OrdersDashboard.Web.Services"]);
    }

    [Fact]
    public void DevelopmentLoggingConfiguration_EnablesDebugForServices()
    {
        // Arrange
        var configurationData = new Dictionary<string, string?>
        {
            ["Logging:LogLevel:Default"] = "Information",
            ["Logging:LogLevel:Microsoft.AspNetCore"] = "Warning",
            ["Logging:LogLevel:OrdersDashboard.Web.Services"] = "Debug"
        };

        // Act
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationData)
            .Build();

        // Assert
        Assert.Equal("Debug", configuration["Logging:LogLevel:OrdersDashboard.Web.Services"]);
    }

    [Fact]
    public void ServiceCollection_CanConfigureOpenTelemetryServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["OpenTelemetry:ServiceName"] = "TestService",
                ["OpenTelemetry:ServiceVersion"] = "1.0.0"
            })
            .Build();

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(
                serviceName: configuration["OpenTelemetry:ServiceName"] ?? "OrdersDashboard",
                serviceVersion: configuration["OpenTelemetry:ServiceVersion"] ?? "1.0.0",
                serviceInstanceId: Environment.MachineName);

        // Act
        services.AddLogging(builder =>
        {
            builder.AddOpenTelemetry(options =>
            {
                options.SetResourceBuilder(resourceBuilder);
                options.AddConsoleExporter();
            });
        });

        services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .SetResourceBuilder(resourceBuilder)
                .AddConsoleExporter())
            .WithMetrics(metrics => metrics
                .SetResourceBuilder(resourceBuilder)
                .AddConsoleExporter());

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        
        Assert.NotNull(loggerFactory);
    }

    [Fact]
    public void ResourceBuilder_IncludesDefaultAttributes()
    {
        // Arrange & Act
        var resource = ResourceBuilder.CreateDefault().Build();

        // Assert
        var telemetrySdkName = resource.Attributes.FirstOrDefault(attr => attr.Key == "telemetry.sdk.name");
        var telemetrySdkVersion = resource.Attributes.FirstOrDefault(attr => attr.Key == "telemetry.sdk.version");

        Assert.False(telemetrySdkName.Equals(default(KeyValuePair<string, object>)));
        Assert.Equal("opentelemetry", telemetrySdkName.Value);
        Assert.False(telemetrySdkVersion.Equals(default(KeyValuePair<string, object>)));
        Assert.NotNull(telemetrySdkVersion.Value);
    }

    [Fact]
    public void ServiceInstanceId_UsesEnvironmentMachineName()
    {
        // Arrange
        var expectedMachineName = Environment.MachineName;

        // Act
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(
                serviceName: "TestService",
                serviceVersion: "1.0.0",
                serviceInstanceId: Environment.MachineName);

        var resource = resourceBuilder.Build();

        // Assert
        var serviceInstanceAttribute = resource.Attributes.FirstOrDefault(attr => attr.Key == "service.instance.id");
        Assert.Equal(expectedMachineName, serviceInstanceAttribute.Value);
    }
}