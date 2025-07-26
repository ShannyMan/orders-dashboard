using OrdersDashboard.Web.Models;
using OrdersDashboard.Web.Services;
using Microsoft.Extensions.Logging;

namespace OrdersDashboard.Tests;

public class OrderServiceTests
{
    private readonly ILogger<OrderService> _mockLogger;

    public OrderServiceTests()
    {
        _mockLogger = new LoggerFactory().CreateLogger<OrderService>();
    }

    [Fact]
    public async Task GetOrdersAsync_ReturnsCorrectNumberOfOrders()
    {
        // Arrange
        var orderService = new OrderService(_mockLogger);

        // Act
        var orders = await orderService.GetOrdersAsync();

        // Assert
        Assert.Equal(10, orders.Count());
    }

    [Fact]
    public async Task GetOrdersAsync_ReturnsOrdersWithExpectedProperties()
    {
        // Arrange
        var orderService = new OrderService(_mockLogger);

        // Act
        var orders = await orderService.GetOrdersAsync();
        var firstOrder = orders.First();

        // Assert
        Assert.True(firstOrder.OrderNumber > 0);
        Assert.NotEmpty(firstOrder.StoreId);
        Assert.NotEmpty(firstOrder.StoreName);
        Assert.True(firstOrder.NumberOfItems > 0);
        Assert.NotEmpty(firstOrder.FulfillmentPartner);
        Assert.NotEqual(default(DateTime), firstOrder.OrderDate);
    }

    [Fact]
    public async Task GetDashboardMetricsAsync_ReturnsValidMetrics()
    {
        // Arrange
        var orderService = new OrderService(_mockLogger);

        // Act
        var metrics = await orderService.GetDashboardMetricsAsync();

        // Assert
        Assert.NotNull(metrics);
        Assert.True(metrics.PlacedOrdersToday >= 0);
        Assert.True(metrics.Average7DayPlacedOrders >= 0);
        Assert.True(metrics.CompletedOrders >= 0);
        Assert.True(metrics.RedLights >= 0);
    }

    [Fact]
    public async Task GetDashboardMetricsAsync_MetricsAddUpToTotalOrders()
    {
        // Arrange
        var orderService = new OrderService(_mockLogger);

        // Act
        var orders = await orderService.GetOrdersAsync();
        var metrics = await orderService.GetDashboardMetricsAsync();

        // Assert
        var actualCompleted = orders.Count(o => o.Status == OrderStatus.Completed);
        var actualCanceled = orders.Count(o => o.Status == OrderStatus.Canceled);
        
        Assert.Equal(actualCompleted, metrics.CompletedOrders);
        Assert.Equal(actualCanceled, metrics.RedLights);
    }

    [Fact]
    public async Task GetOrdersAsync_ContainsVariousOrderStatuses()
    {
        // Arrange
        var orderService = new OrderService(_mockLogger);

        // Act
        var orders = await orderService.GetOrdersAsync();
        var distinctStatuses = orders.Select(o => o.Status).Distinct();

        // Assert
        Assert.True(distinctStatuses.Count() > 1, "Orders should have various statuses");
    }

    [Fact]
    public async Task GetOrdersAsync_ContainsVariousFulfillmentTypes()
    {
        // Arrange
        var orderService = new OrderService(_mockLogger);

        // Act
        var orders = await orderService.GetOrdersAsync();
        var distinctTypes = orders.Select(o => o.FulfillmentType).Distinct();

        // Assert
        Assert.True(distinctTypes.Count() > 1, "Orders should have various fulfillment types");
    }
}