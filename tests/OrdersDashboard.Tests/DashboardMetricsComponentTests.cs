using Bunit;
using OrdersDashboard.Web.Components;
using OrdersDashboard.Web.Models;

namespace OrdersDashboard.Tests;

/// <summary>
/// Unit tests for the DashboardMetricsComponent.
/// </summary>
public class DashboardMetricsComponentTests : TestContext
{
    [Fact]
    public void DashboardMetricsComponent_RendersDefaultMetrics()
    {
        // Arrange
        var metrics = new DashboardMetrics();

        // Act
        var component = RenderComponent<DashboardMetricsComponent>(parameters => parameters
            .Add(p => p.Metrics, metrics));

        // Assert
        Assert.Contains("0", component.Markup); // PlacedOrdersToday
        Assert.Contains("Placed Orders Today", component.Markup);
        Assert.Contains("Average 7-Day Placed Orders", component.Markup);
        Assert.Contains("Completed Orders", component.Markup);
        Assert.Contains("Red Lights", component.Markup);
    }

    [Fact]
    public void DashboardMetricsComponent_RendersCustomMetrics()
    {
        // Arrange
        var metrics = new DashboardMetrics
        {
            PlacedOrdersToday = 15,
            Average7DayPlacedOrders = 12.5,
            CompletedOrders = 250,
            RedLights = 3
        };

        // Act
        var component = RenderComponent<DashboardMetricsComponent>(parameters => parameters
            .Add(p => p.Metrics, metrics));

        // Assert
        Assert.Contains("15", component.Markup); // PlacedOrdersToday
        Assert.Contains("12.5", component.Markup); // Average7DayPlacedOrders
        Assert.Contains("250", component.Markup); // CompletedOrders
        Assert.Contains("3", component.Markup); // RedLights
    }

    [Fact]
    public void DashboardMetricsComponent_HasCorrectCssClasses()
    {
        // Arrange
        var metrics = new DashboardMetrics();

        // Act
        var component = RenderComponent<DashboardMetricsComponent>(parameters => parameters
            .Add(p => p.Metrics, metrics));

        // Assert
        Assert.Contains("card", component.Markup);
        Assert.Contains("metric-card", component.Markup);
        Assert.Contains("text-primary", component.Markup);
        Assert.Contains("text-info", component.Markup);
        Assert.Contains("text-success", component.Markup);
        Assert.Contains("text-danger", component.Markup);
    }

    [Fact]
    public void DashboardMetricsComponent_HasBootstrapIcons()
    {
        // Arrange
        var metrics = new DashboardMetrics();

        // Act
        var component = RenderComponent<DashboardMetricsComponent>(parameters => parameters
            .Add(p => p.Metrics, metrics));

        // Assert
        Assert.Contains("bi bi-bag-plus", component.Markup);
        Assert.Contains("bi bi-graph-up", component.Markup);
        Assert.Contains("bi bi-check-circle", component.Markup);
        Assert.Contains("bi bi-exclamation-triangle", component.Markup);
    }

    [Fact]
    public void DashboardMetricsComponent_HasFourMetricCards()
    {
        // Arrange
        var metrics = new DashboardMetrics();

        // Act
        var component = RenderComponent<DashboardMetricsComponent>(parameters => parameters
            .Add(p => p.Metrics, metrics));

        // Assert
        var cards = component.FindAll(".card");
        Assert.Equal(4, cards.Count);
    }
}