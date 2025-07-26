using OrdersDashboard.Web.Models;

namespace OrdersDashboard.Web.Services;

/// <summary>
/// Defines the contract for order-related operations
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Retrieves dashboard metrics asynchronously
    /// </summary>
    /// <returns>A task containing the dashboard metrics</returns>
    Task<DashboardMetrics> GetDashboardMetricsAsync();
    
    /// <summary>
    /// Retrieves all orders asynchronously
    /// </summary>
    /// <returns>A task containing an enumerable collection of orders</returns>
    Task<IEnumerable<Order>> GetOrdersAsync();
}

/// <summary>
/// Provides order-related operations with mock data for demonstration purposes
/// </summary>
public class OrderService : IOrderService
{
    /// <summary>
    /// Collection of sample orders for demonstration
    /// </summary>
    private readonly List<Order> _orders;

    /// <summary>
    /// Initializes a new instance of the OrderService class
    /// </summary>
    public OrderService()
    {
        _orders = GenerateSampleOrders();
    }

    /// <summary>
    /// Calculates and returns dashboard metrics based on current orders
    /// </summary>
    /// <returns>A task containing the calculated dashboard metrics</returns>
    public Task<DashboardMetrics> GetDashboardMetricsAsync()
    {
        var today = DateTime.Today;
        var placedToday = _orders.Count(o => o.OrderDate.Date == today && o.Status == OrderStatus.Placed);
        var sevenDaysAgo = today.AddDays(-7);
        var last7Days = _orders.Where(o => o.OrderDate >= sevenDaysAgo && o.Status == OrderStatus.Placed);
        var average7Day = last7Days.Count() / 7.0;
        var completed = _orders.Count(o => o.Status == OrderStatus.Completed);
        var redLights = _orders.Count(o => o.Status == OrderStatus.Canceled);

        var metrics = new DashboardMetrics
        {
            PlacedOrdersToday = placedToday,
            Average7DayPlacedOrders = Math.Round(average7Day, 1),
            CompletedOrders = completed,
            RedLights = redLights
        };

        return Task.FromResult(metrics);
    }

    /// <summary>
    /// Retrieves all orders in the system
    /// </summary>
    /// <returns>A task containing all orders</returns>
    public Task<IEnumerable<Order>> GetOrdersAsync()
    {
        return Task.FromResult<IEnumerable<Order>>(_orders);
    }

    /// <summary>
    /// Generates sample order data for demonstration purposes
    /// </summary>
    /// <returns>A list of sample orders</returns>
    private List<Order> GenerateSampleOrders()
    {
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

        return orders;
    }
}