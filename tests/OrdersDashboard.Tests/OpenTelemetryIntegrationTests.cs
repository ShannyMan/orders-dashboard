using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrdersDashboard.Web.Services;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OrdersDashboard.Tests.Helpers;

namespace OrdersDashboard.Tests;

public class OpenTelemetryIntegrationTests
{
    [Fact]
    public void OrderService_WithOpenTelemetryLogger_ProducesStructuredLogs()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["OpenTelemetry:ServiceName"] = "OrdersDashboard",
                ["OpenTelemetry:ServiceVersion"] = "1.0.0"
            })
            .Build();

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(
                serviceName: configuration["OpenTelemetry:ServiceName"] ?? "OrdersDashboard",
                serviceVersion: configuration["OpenTelemetry:ServiceVersion"] ?? "1.0.0",
                serviceInstanceId: Environment.MachineName);

        var logger = new TestLogger<OrderService>();
        services.AddSingleton<ILogger<OrderService>>(logger);
        services.AddSingleton<IOrderService, OrderService>();

        var serviceProvider = services.BuildServiceProvider();

        // Act
        var orderService = serviceProvider.GetRequiredService<IOrderService>();

        // Assert
        var logEntries = logger.LogEntries;
        var initEntry = logEntries.FirstOrDefault(entry => 
            entry.Message.Contains("OrderService initialized"));

        Assert.NotNull(initEntry);
        Assert.Equal(LogLevel.Information, initEntry.Level);
        Assert.Equal("OrdersDashboard.Web.Services.OrderService", initEntry.Category);
    }

    [Fact]
    public async Task OrderService_AllOperations_ProduceExpectedLogStructure()
    {
        // Arrange
        var logger = new TestLogger<OrderService>();
        var orderService = new OrderService(logger);

        // Clear initial constructor logs
        logger.Clear();

        // Act
        var orders = await orderService.GetOrdersAsync();
        var metrics = await orderService.GetDashboardMetricsAsync();

        // Assert
        var logEntries = logger.LogEntries;
        
        // Verify we have logs from all operations
        Assert.Contains(logEntries, entry => entry.Message.Contains("Retrieving all orders"));
        Assert.Contains(logEntries, entry => entry.Message.Contains("Calculating dashboard metrics"));
        Assert.Contains(logEntries, entry => entry.Message.Contains("Dashboard metrics calculated"));

        // Verify log structure consistency
        Assert.All(logEntries, entry =>
        {
            Assert.Equal("OrdersDashboard.Web.Services.OrderService", entry.Category);
            Assert.True(entry.Level >= LogLevel.Information);
            Assert.NotNull(entry.Message);
        });

        // Verify structured data is present in metrics log
        var metricsLog = logEntries.First(entry => entry.Message.Contains("Dashboard metrics calculated"));
        Assert.NotNull(metricsLog.StructuredState);
        Assert.True(metricsLog.StructuredState.Count >= 4); // Should have PlacedToday, Average7Day, Completed, RedLights
    }

    [Fact]
    public void ResourceBuilder_WithConfiguration_CreatesCorrectServiceAttributes()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["OpenTelemetry:ServiceName"] = "OrdersDashboard",
                ["OpenTelemetry:ServiceVersion"] = "1.0.0"
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
        var attributes = resource.Attributes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        
        Assert.Equal("OrdersDashboard", attributes["service.name"]);
        Assert.Equal("1.0.0", attributes["service.version"]);
        Assert.Equal(Environment.MachineName, attributes["service.instance.id"]);
        Assert.Equal("opentelemetry", attributes["telemetry.sdk.name"]);
        Assert.Contains("telemetry.sdk.version", attributes.Keys);
    }

    [Fact]
    public void OrderService_DebugLogging_CapturesGenerationDetails()
    {
        // Arrange
        var logger = new TestLogger<OrderService>();

        // Act
        var orderService = new OrderService(logger);

        // Assert
        var logEntries = logger.LogEntries;
        
        // Look for debug logs (constructor calls GenerateSampleOrders which logs at Debug level)
        var debugEntries = logEntries.Where(entry => entry.Level == LogLevel.Debug).ToList();
        
        // Should have debug logs for generation process
        Assert.Contains(debugEntries, entry => entry.Message.Contains("Generating sample order data"));
        Assert.Contains(debugEntries, entry => entry.Message.Contains("Generated") && entry.Message.Contains("sample orders"));
    }

    [Fact]
    public void OrderService_LoggerCategory_MatchesServiceNamespace()
    {
        // Arrange
        var logger = new TestLogger<OrderService>();

        // Act
        var orderService = new OrderService(logger);

        // Assert
        var logEntries = logger.LogEntries;
        Assert.All(logEntries, entry =>
            Assert.Equal("OrdersDashboard.Web.Services.OrderService", entry.Category));
    }

    [Fact]
    public async Task OrderService_ConcurrentOperations_ProducesCoherentLogs()
    {
        // Arrange
        var logger = new TestLogger<OrderService>();
        var orderService = new OrderService(logger);

        // Clear constructor logs
        logger.Clear();

        // Act - Run operations concurrently
        var tasks = new Task[]
        {
            orderService.GetOrdersAsync(),
            orderService.GetDashboardMetricsAsync(),
            orderService.GetOrdersAsync(),
            orderService.GetDashboardMetricsAsync()
        };

        await Task.WhenAll(tasks);

        // Assert
        var logEntries = logger.LogEntries;
        
        // Should have multiple log entries for each operation type
        var retrievalLogs = logEntries.Where(entry => entry.Message.Contains("Retrieving all orders")).ToList();
        var calculationLogs = logEntries.Where(entry => entry.Message.Contains("Calculating dashboard metrics")).ToList();
        
        Assert.Equal(2, retrievalLogs.Count);
        Assert.Equal(2, calculationLogs.Count);
        
        // All logs should maintain proper category
        Assert.All(logEntries, entry =>
            Assert.Equal("OrdersDashboard.Web.Services.OrderService", entry.Category));
    }
}