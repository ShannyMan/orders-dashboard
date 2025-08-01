namespace OrdersDashboard.Web.Models;

/// <summary>
/// Configuration options for Azure Search service
/// </summary>
public class AzureSearchOptions
{
    /// <summary>
    /// Configuration section name for Azure Search settings
    /// </summary>
    public const string SectionName = "AzureSearch";

    /// <summary>
    /// Gets or sets the Azure Search service connection string
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Azure Search index name for orders
    /// </summary>
    public string OrdersIndexName { get; set; } = "orders";
}