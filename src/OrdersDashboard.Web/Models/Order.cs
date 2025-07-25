namespace OrdersDashboard.Web.Models;

public class Order
{
    public int OrderNumber { get; set; }
    public OrderStatus Status { get; set; }
    public string StoreId { get; set; } = string.Empty;
    public string StoreName { get; set; } = string.Empty;
    public int NumberOfItems { get; set; }
    public FulfillmentType FulfillmentType { get; set; }
    public string FulfillmentPartner { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
}

public enum OrderStatus
{
    Placed,
    Fulfillment,
    Completed,
    Canceled
}

public enum FulfillmentType
{
    Pickup,
    Delivery,
    Shipping
}