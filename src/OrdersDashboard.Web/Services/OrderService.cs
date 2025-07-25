using OrdersDashboard.Web.Models;

namespace OrdersDashboard.Web.Services;

public interface IOrderService
{
    Task<DashboardMetrics> GetDashboardMetricsAsync();
    Task<IEnumerable<Order>> GetOrdersAsync();
}

public class OrderService : IOrderService
{
    private readonly List<Order> _orders;

    public OrderService()
    {
        _orders = GenerateSampleOrders();
    }

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

    public Task<IEnumerable<Order>> GetOrdersAsync()
    {
        return Task.FromResult<IEnumerable<Order>>(_orders);
    }

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