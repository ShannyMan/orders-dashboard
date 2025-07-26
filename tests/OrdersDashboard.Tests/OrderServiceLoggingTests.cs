using Microsoft.Extensions.Logging;
using OrdersDashboard.Web.Services;
using OrdersDashboard.Tests.Helpers;

namespace OrdersDashboard.Tests;

public class OrderServiceLoggingTests
{
    [Fact]
    public void Constructor_LogsServiceInitialization()
    {
        // Arrange
        var logger = new TestLogger<OrderService>();

        // Act
        var orderService = new OrderService(logger);

        // Assert
        var logEntries = logger.LogEntries;
        var initLogEntry = logEntries.FirstOrDefault(entry => 
            entry.Message.Contains("OrderService initialized"));
        
        Assert.NotNull(initLogEntry);
        Assert.Equal(LogLevel.Information, initLogEntry.Level);
        Assert.Contains("OrderService initialized with", initLogEntry.Message);
        Assert.Contains("sample orders", initLogEntry.Message);
        
        // Verify structured logging data
        var orderCountValue = initLogEntry.StructuredState.FirstOrDefault(kvp => kvp.Key == "OrderCount");
        Assert.False(orderCountValue.Equals(default(KeyValuePair<string, object?>)));
        Assert.Equal(10, orderCountValue.Value);
    }

    [Fact]
    public async Task GetDashboardMetricsAsync_LogsMetricsCalculation()
    {
        // Arrange
        var logger = new TestLogger<OrderService>();
        var orderService = new OrderService(logger);

        // Clear initial logs
        logger.Clear();

        // Act
        await orderService.GetDashboardMetricsAsync();

        // Assert
        var logEntries = logger.LogEntries;
        
        // Verify calculation start log
        var calcStartEntry = logEntries.FirstOrDefault(entry => 
            entry.Message.Contains("Calculating dashboard metrics"));
        Assert.NotNull(calcStartEntry);
        Assert.Equal(LogLevel.Information, calcStartEntry.Level);

        // Verify metrics calculation result log
        var metricsResultEntry = logEntries.FirstOrDefault(entry => 
            entry.Message.Contains("Dashboard metrics calculated"));
        Assert.NotNull(metricsResultEntry);
        Assert.Equal(LogLevel.Information, metricsResultEntry.Level);
        
        // Verify structured logging contains metric values
        var structuredState = metricsResultEntry.StructuredState;
        Assert.NotNull(structuredState);
        Assert.Contains(structuredState, kvp => kvp.Key == "PlacedToday");
        Assert.Contains(structuredState, kvp => kvp.Key == "Average7Day");
        Assert.Contains(structuredState, kvp => kvp.Key == "Completed");
        Assert.Contains(structuredState, kvp => kvp.Key == "RedLights");
    }

    [Fact]
    public async Task GetOrdersAsync_LogsOrderRetrieval()
    {
        // Arrange
        var logger = new TestLogger<OrderService>();
        var orderService = new OrderService(logger);

        // Clear initial logs
        logger.Clear();

        // Act
        await orderService.GetOrdersAsync();

        // Assert
        var logEntries = logger.LogEntries;
        var retrievalEntry = logEntries.FirstOrDefault(entry => 
            entry.Message.Contains("Retrieving all orders"));
        
        Assert.NotNull(retrievalEntry);
        Assert.Equal(LogLevel.Information, retrievalEntry.Level);
    }

    [Fact]
    public void GenerateSampleOrders_LogsDebugInformation()
    {
        // Arrange
        var logger = new TestLogger<OrderService>();

        // Act
        var orderService = new OrderService(logger);

        // Assert
        var logEntries = logger.LogEntries;
        
        // Verify debug log for generating sample data
        var generateEntry = logEntries.FirstOrDefault(entry => 
            entry.Message.Contains("Generating sample order data"));
        Assert.NotNull(generateEntry);
        Assert.Equal(LogLevel.Debug, generateEntry.Level);

        // Verify debug log for generated count
        var countEntry = logEntries.FirstOrDefault(entry => 
            entry.Message.Contains("Generated") && entry.Message.Contains("sample orders"));
        Assert.NotNull(countEntry);
        Assert.Equal(LogLevel.Debug, countEntry.Level);
        
        // Verify structured logging for order count
        var orderCountValue = countEntry.StructuredState.FirstOrDefault(kvp => kvp.Key == "OrderCount");
        Assert.False(orderCountValue.Equals(default(KeyValuePair<string, object?>)));
        Assert.Equal(10, orderCountValue.Value);
    }

    [Fact]
    public void Constructor_ValidatesLoggerDependency()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new OrderService(null!));
    }

    [Fact]
    public async Task MultipleOperations_LogsWithConsistentLogger()
    {
        // Arrange
        var logger = new TestLogger<OrderService>();
        var orderService = new OrderService(logger);

        // Clear initial logs
        logger.Clear();

        // Act
        await orderService.GetOrdersAsync();
        await orderService.GetDashboardMetricsAsync();

        // Assert
        var logEntries = logger.LogEntries;
        
        // Verify we have logs from both operations
        Assert.Contains(logEntries, entry => entry.Message.Contains("Retrieving all orders"));
        Assert.Contains(logEntries, entry => entry.Message.Contains("Calculating dashboard metrics"));
        
        // Verify all logs are at Information level or higher
        Assert.All(logEntries, entry => Assert.True(entry.Level >= LogLevel.Information));
    }

    [Fact]
    public void LoggerCategory_MatchesExpectedNamespace()
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
}