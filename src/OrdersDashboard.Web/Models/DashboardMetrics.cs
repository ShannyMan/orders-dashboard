namespace OrdersDashboard.Web.Models;

/// <summary>
/// Contains metrics and statistics for the orders dashboard
/// </summary>
public class DashboardMetrics
{
    /// <summary>
    /// Gets or sets the number of orders placed today
    /// </summary>
    public int PlacedOrdersToday { get; set; }
    
    /// <summary>
    /// Gets or sets the average number of orders placed per day over the last 7 days
    /// </summary>
    public double Average7DayPlacedOrders { get; set; }
    
    /// <summary>
    /// Gets or sets the total number of completed orders
    /// </summary>
    public int CompletedOrders { get; set; }
    
    /// <summary>
    /// Gets or sets the number of canceled orders (red lights/alerts)
    /// </summary>
    public int RedLights { get; set; }
}