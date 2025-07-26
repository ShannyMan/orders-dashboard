namespace OrdersDashboard.Web.Models;

/// <summary>
/// Represents the method used to fulfill an order
/// </summary>
public enum FulfillmentType
{
    /// <summary>
    /// Customer picks up the order from a store location
    /// </summary>
    Pickup,
    
    /// <summary>
    /// Order is delivered to the customer's location
    /// </summary>
    Delivery,
    
    /// <summary>
    /// Order is shipped to the customer via courier service
    /// </summary>
    Shipping
}