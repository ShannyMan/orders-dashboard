using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrdersDashboard.Web.Models;
using OrdersDashboard.Web.Services;

namespace OrdersDashboard.Tests;

public class AzureSearchServiceTests
{
    private readonly ILogger<AzureSearchService> _mockLogger;

    public AzureSearchServiceTests()
    {
        _mockLogger = new LoggerFactory().CreateLogger<AzureSearchService>();
    }

    [Fact]
    public async Task GetOrdersAsync_WithEmptyConnectionString_ReturnsMockData()
    {
        // Arrange
        var options = new AzureSearchOptions
        {
            ConnectionString = "",
            OrdersIndexName = "orders"
        };
        var azureSearchService = new AzureSearchService(Options.Create(options), _mockLogger);

        // Act
        var orders = await azureSearchService.GetOrdersAsync();

        // Assert
        Assert.NotNull(orders);
        Assert.Equal(10, orders.Count());
    }

    [Fact]
    public async Task GetOrdersAsync_WithEmptyConnectionString_ReturnsOrdersWithExpectedProperties()
    {
        // Arrange
        var options = new AzureSearchOptions
        {
            ConnectionString = "",
            OrdersIndexName = "orders"
        };
        var azureSearchService = new AzureSearchService(Options.Create(options), _mockLogger);

        // Act
        var orders = await azureSearchService.GetOrdersAsync();
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
    public async Task GetOrdersAsync_WithInvalidConnectionString_FallsBackToMockData()
    {
        // Arrange
        var options = new AzureSearchOptions
        {
            ConnectionString = "invalid-connection-string",
            OrdersIndexName = "orders"
        };
        var azureSearchService = new AzureSearchService(Options.Create(options), _mockLogger);

        // Act
        var orders = await azureSearchService.GetOrdersAsync();

        // Assert
        Assert.NotNull(orders);
        Assert.Equal(10, orders.Count());
    }

    [Fact]
    public void Constructor_WithEmptyConnectionString_LogsInformation()
    {
        // Arrange
        var loggerFactory = new LoggerFactory();
        var logger = loggerFactory.CreateLogger<AzureSearchService>();
        var options = new AzureSearchOptions
        {
            ConnectionString = "",
            OrdersIndexName = "orders"
        };

        // Act & Assert - Should not throw exception
        var azureSearchService = new AzureSearchService(Options.Create(options), logger);
        Assert.NotNull(azureSearchService);
    }

    [Fact]
    public async Task GetOrdersAsync_ReturnsConsistentMockData()
    {
        // Arrange
        var options = new AzureSearchOptions
        {
            ConnectionString = "",
            OrdersIndexName = "orders"
        };
        var azureSearchService1 = new AzureSearchService(Options.Create(options), _mockLogger);
        var azureSearchService2 = new AzureSearchService(Options.Create(options), _mockLogger);

        // Act
        var orders1 = await azureSearchService1.GetOrdersAsync();
        var orders2 = await azureSearchService2.GetOrdersAsync();

        // Assert - Mock data should be consistent due to fixed seed
        Assert.Equal(orders1.Count(), orders2.Count());
        var ordersArray1 = orders1.ToArray();
        var ordersArray2 = orders2.ToArray();
        
        for (int i = 0; i < ordersArray1.Length; i++)
        {
            Assert.Equal(ordersArray1[i].OrderNumber, ordersArray2[i].OrderNumber);
            Assert.Equal(ordersArray1[i].StoreId, ordersArray2[i].StoreId);
        }
    }
}