using OrdersDashboard.Web.Models;

namespace OrdersDashboard.Tests;

public class OrderStatusTests
{
    [Theory]
    [InlineData(OrderStatus.Placed, 0)]
    [InlineData(OrderStatus.Fulfillment, 1)]
    [InlineData(OrderStatus.Completed, 2)]
    [InlineData(OrderStatus.Canceled, 3)]
    public void OrderStatus_EnumValues_AreCorrect(OrderStatus status, int expectedValue)
    {
        // Act & Assert
        Assert.Equal(expectedValue, (int)status);
    }

    [Fact]
    public void OrderStatus_AllValues_AreAccessible()
    {
        // Arrange
        var expectedStatuses = new[] { OrderStatus.Placed, OrderStatus.Fulfillment, OrderStatus.Completed, OrderStatus.Canceled };

        // Act
        var actualStatuses = Enum.GetValues<OrderStatus>();

        // Assert
        Assert.Equal(expectedStatuses.Length, actualStatuses.Length);
        foreach (var status in expectedStatuses)
        {
            Assert.Contains(status, actualStatuses);
        }
    }
}