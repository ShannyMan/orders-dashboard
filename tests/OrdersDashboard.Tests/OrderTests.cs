using OrdersDashboard.Web.Models;

namespace OrdersDashboard.Tests;

public class OrderTests
{
    [Fact]
    public void Order_DefaultValues_AreCorrect()
    {
        // Arrange & Act
        var order = new Order();

        // Assert
        Assert.Equal(0, order.OrderNumber);
        Assert.Equal(OrderStatus.Placed, order.Status);
        Assert.Equal(string.Empty, order.StoreId);
        Assert.Equal(string.Empty, order.StoreName);
        Assert.Equal(0, order.NumberOfItems);
        Assert.Equal(FulfillmentType.Pickup, order.FulfillmentType);
        Assert.Equal(string.Empty, order.FulfillmentPartner);
        Assert.Equal(default(DateTime), order.OrderDate);
    }

    [Fact]
    public void Order_PropertiesCanBeSet()
    {
        // Arrange
        var order = new Order();
        var expectedDate = new DateTime(2024, 1, 15);

        // Act
        order.OrderNumber = 1001;
        order.Status = OrderStatus.Completed;
        order.StoreId = "ST001";
        order.StoreName = "Test Store";
        order.NumberOfItems = 5;
        order.FulfillmentType = FulfillmentType.Delivery;
        order.FulfillmentPartner = "FedEx";
        order.OrderDate = expectedDate;

        // Assert
        Assert.Equal(1001, order.OrderNumber);
        Assert.Equal(OrderStatus.Completed, order.Status);
        Assert.Equal("ST001", order.StoreId);
        Assert.Equal("Test Store", order.StoreName);
        Assert.Equal(5, order.NumberOfItems);
        Assert.Equal(FulfillmentType.Delivery, order.FulfillmentType);
        Assert.Equal("FedEx", order.FulfillmentPartner);
        Assert.Equal(expectedDate, order.OrderDate);
    }
}