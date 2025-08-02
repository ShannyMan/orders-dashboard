using Azure;
using Azure.Search.Documents;
using Microsoft.Extensions.Options;
using OrdersDashboard.Web.Models;

namespace OrdersDashboard.Web.Services;

/// <summary>
/// Provides order retrieval operations from Azure Search service with fallback to mock data
/// </summary>
public class AzureSearchService : IAzureSearchService
{
    private readonly AzureSearchOptions _options;
    private readonly ILogger<AzureSearchService> _logger;
    private readonly SearchClient? _searchClient;

    /// <summary>
    /// Initializes a new instance of the AzureSearchService class
    /// </summary>
    /// <param name="options">Azure Search configuration options</param>
    /// <param name="logger">Logger instance</param>
    public AzureSearchService(IOptions<AzureSearchOptions> options, ILogger<AzureSearchService> logger)
    {
        _options = options.Value;
        _logger = logger;

        // Only initialize SearchClient if connection string is provided
        if (!string.IsNullOrEmpty(_options.ConnectionString))
        {
            try
            {
                var searchClientOptions = new SearchClientOptions();
                _searchClient = new SearchClient(
                    new Uri(_options.ConnectionString),
                    _options.OrdersIndexName,
                    new AzureKeyCredential(""), // We'll need to extract the API key from connection string in a real implementation
                    searchClientOptions);
                
                _logger.LogInformation("Azure Search client initialized for index '{IndexName}'", _options.OrdersIndexName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to initialize Azure Search client. Falling back to mock data.");
                _searchClient = null;
            }
        }
        else
        {
            _logger.LogInformation("Azure Search connection string not configured. Using mock data.");
        }
    }

    /// <summary>
    /// Retrieves all orders from Azure Search or returns mock data if not configured
    /// </summary>
    /// <returns>A task containing all orders</returns>
    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        if (_searchClient != null)
        {
            try
            {
                _logger.LogInformation("Retrieving orders from Azure Search");
                return await GetOrdersFromAzureSearchAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders from Azure Search. Falling back to mock data.");
            }
        }

        _logger.LogInformation("Using mock data for orders");
        return await GetMockOrdersAsync();
    }

    /// <summary>
    /// Retrieves orders from Azure Search service
    /// </summary>
    /// <returns>A task containing orders from Azure Search</returns>
    private async Task<IEnumerable<Order>> GetOrdersFromAzureSearchAsync()
    {
        if (_searchClient == null)
            return await GetMockOrdersAsync();

        // In a real implementation, this would perform the actual Azure Search query
        // For now, we'll return mock data since we don't have a real Azure Search service configured
        _logger.LogInformation("Would retrieve orders from Azure Search index '{IndexName}' here", _options.OrdersIndexName);
        
        // Simulate async operation
        await Task.Delay(100);
        
        // Return mock data for now - this would be replaced with actual Azure Search results
        return await GetMockOrdersAsync();
    }

    /// <summary>
    /// Generates mock order data when Azure Search is not available
    /// </summary>
    /// <returns>A task containing mock orders</returns>
    private Task<IEnumerable<Order>> GetMockOrdersAsync()
    {
        _logger.LogDebug("Generating mock order data");
        
        var random = new Random(42); // Fixed seed for consistent data
        var stores = new[]
        {
            ("ST001", "Downtown Store"),
            ("ST002", "Mall Location"),
            ("ST003", "Suburban Branch"),
            ("ST004", "Airport Store"),
            ("ST005", "City Center")
        };

        var orders = new List<Order>();
        var today = DateTime.Today;

        for (int i = 1; i <= 10; i++)
        {
            var store = stores[random.Next(stores.Length)];
            var fulfillmentType = (FulfillmentType)random.Next(0, 3);
            var fulfillmentPartner = fulfillmentType switch
            {
                FulfillmentType.Delivery => random.NextDouble() > 0.5 ? "Shipt" : "FedEx",
                FulfillmentType.Pickup => "Mi9",
                FulfillmentType.Shipping => "FedEx",
                _ => "Unknown"
            };

            orders.Add(new Order
            {
                OrderNumber = 1000 + i,
                Status = (OrderStatus)random.Next(0, 4),
                StoreId = store.Item1,
                StoreName = store.Item2,
                NumberOfItems = random.Next(1, 15),
                FulfillmentType = fulfillmentType,
                FulfillmentPartner = fulfillmentPartner,
                OrderDate = today.AddDays(-random.Next(0, 14))
            });
        }

        _logger.LogDebug("Generated {OrderCount} mock orders", orders.Count);
        return Task.FromResult<IEnumerable<Order>>(orders);
    }
}