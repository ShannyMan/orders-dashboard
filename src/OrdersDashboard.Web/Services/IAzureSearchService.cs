using OrdersDashboard.Web.Models;

namespace OrdersDashboard.Web.Services;

/// <summary>
/// Defines the contract for Azure Search operations for order retrieval
/// </summary>
public interface IAzureSearchService
{
    /// <summary>
    /// Retrieves all orders from Azure Search or mock data if not configured
    /// </summary>
    /// <returns>A task containing an enumerable collection of orders</returns>
    Task<IEnumerable<Order>> GetOrdersAsync();
}