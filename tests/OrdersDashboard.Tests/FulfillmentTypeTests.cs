using OrdersDashboard.Web.Models;

namespace OrdersDashboard.Tests;

public class FulfillmentTypeTests
{
    [Theory]
    [InlineData(FulfillmentType.Pickup, 0)]
    [InlineData(FulfillmentType.Delivery, 1)]
    [InlineData(FulfillmentType.Shipping, 2)]
    public void FulfillmentType_EnumValues_AreCorrect(FulfillmentType type, int expectedValue)
    {
        // Act & Assert
        Assert.Equal(expectedValue, (int)type);
    }

    [Fact]
    public void FulfillmentType_AllValues_AreAccessible()
    {
        // Arrange
        var expectedTypes = new[] { FulfillmentType.Pickup, FulfillmentType.Delivery, FulfillmentType.Shipping };

        // Act
        var actualTypes = Enum.GetValues<FulfillmentType>();

        // Assert
        Assert.Equal(expectedTypes.Length, actualTypes.Length);
        foreach (var type in expectedTypes)
        {
            Assert.Contains(type, actualTypes);
        }
    }
}