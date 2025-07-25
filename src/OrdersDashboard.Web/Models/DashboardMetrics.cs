namespace OrdersDashboard.Web.Models;

public class DashboardMetrics
{
    public int PlacedOrdersToday { get; set; }
    public double Average7DayPlacedOrders { get; set; }
    public int CompletedOrders { get; set; }
    public int RedLights { get; set; }
}