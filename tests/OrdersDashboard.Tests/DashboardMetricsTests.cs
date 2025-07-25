using OrdersDashboard.Web.Models;

namespace OrdersDashboard.Tests;

public class DashboardMetricsTests
{
    [Fact]
    public void DashboardMetrics_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var metrics = new DashboardMetrics();

        // Assert
        Assert.Equal(0, metrics.PlacedOrdersToday);
        Assert.Equal(0, metrics.Average7DayPlacedOrders);
        Assert.Equal(0, metrics.CompletedOrders);
        Assert.Equal(0, metrics.RedLights);
    }

    [Fact]
    public void DashboardMetrics_PropertiesCanBeSet()
    {
        // Arrange
        var metrics = new DashboardMetrics();

        // Act
        metrics.PlacedOrdersToday = 5;
        metrics.Average7DayPlacedOrders = 3.5;
        metrics.CompletedOrders = 100;
        metrics.RedLights = 2;

        // Assert
        Assert.Equal(5, metrics.PlacedOrdersToday);
        Assert.Equal(3.5, metrics.Average7DayPlacedOrders);
        Assert.Equal(100, metrics.CompletedOrders);
        Assert.Equal(2, metrics.RedLights);
    }
}