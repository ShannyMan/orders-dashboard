using OrdersDashboard.Web.Services;

namespace OrdersDashboard.Tests;

public class CalculatorTests
{
    [Fact]
    public void Add_TwoPositiveNumbers_ReturnsSum()
    {
        // Arrange
        var calculator = new Calculator();
        int a = 5;
        int b = 3;

        // Act
        var result = calculator.Add(a, b);

        // Assert
        Assert.Equal(8, result);
    }

    [Fact]
    public void Subtract_TwoNumbers_ReturnsDifference()
    {
        // Arrange
        var calculator = new Calculator();
        int a = 10;
        int b = 4;

        // Act
        var result = calculator.Subtract(a, b);

        // Assert
        Assert.Equal(6, result);
    }
}