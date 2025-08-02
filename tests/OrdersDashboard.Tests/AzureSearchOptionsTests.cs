using OrdersDashboard.Web.Models;

namespace OrdersDashboard.Tests;

public class AzureSearchOptionsTests
{
    [Fact]
    public void SectionName_ReturnsExpectedValue()
    {
        // Act & Assert
        Assert.Equal("AzureSearch", AzureSearchOptions.SectionName);
    }

    [Fact]
    public void DefaultConstructor_InitializesPropertiesCorrectly()
    {
        // Act
        var options = new AzureSearchOptions();

        // Assert
        Assert.Equal(string.Empty, options.ConnectionString);
        Assert.Equal("orders", options.OrdersIndexName);
    }

    [Fact]
    public void Properties_CanBeSetAndRetrieved()
    {
        // Arrange
        const string connectionString = "test-connection-string";
        const string indexName = "test-index";
        var options = new AzureSearchOptions();

        // Act
        options.ConnectionString = connectionString;
        options.OrdersIndexName = indexName;

        // Assert
        Assert.Equal(connectionString, options.ConnectionString);
        Assert.Equal(indexName, options.OrdersIndexName);
    }

    [Fact]
    public void OrdersIndexName_HasDefaultValue()
    {
        // Act
        var options = new AzureSearchOptions();

        // Assert
        Assert.Equal("orders", options.OrdersIndexName);
    }
}