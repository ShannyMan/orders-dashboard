namespace OrdersDashboard.Web.Models;

/// <summary>
/// Represents the current status of an order in the fulfillment process
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// Order has been placed but not yet started fulfillment
    /// </summary>
    Placed,
    
    /// <summary>
    /// Order is currently being fulfilled
    /// </summary>
    Fulfillment,
    
    /// <summary>
    /// Order has been completed successfully
    /// </summary>
    Completed,
    
    /// <summary>
    /// Order has been canceled
    /// </summary>
    Canceled
}