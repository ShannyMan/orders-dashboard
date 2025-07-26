namespace OrdersDashboard.Web.Models;

/// <summary>
/// Represents an order placed by a customer
/// </summary>
public class Order
{
    /// <summary>
    /// Gets or sets the unique order number
    /// </summary>
    public int OrderNumber { get; set; }
    
    /// <summary>
    /// Gets or sets the current status of the order
    /// </summary>
    public OrderStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the identifier of the store handling the order
    /// </summary>
    public string StoreId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the name of the store handling the order
    /// </summary>
    public string StoreName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the number of items in the order
    /// </summary>
    public int NumberOfItems { get; set; }
    
    /// <summary>
    /// Gets or sets how the order will be fulfilled
    /// </summary>
    public FulfillmentType FulfillmentType { get; set; }
    
    /// <summary>
    /// Gets or sets the partner responsible for fulfilling the order
    /// </summary>
    public string FulfillmentPartner { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the date when the order was placed
    /// </summary>
    public DateTime OrderDate { get; set; }
}