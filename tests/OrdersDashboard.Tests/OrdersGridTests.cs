using Bunit;
using Microsoft.Extensions.DependencyInjection;
using OrdersDashboard.Web.Components;
using OrdersDashboard.Web.Models;

namespace OrdersDashboard.Tests;

/// <summary>
/// Unit tests for the OrdersGrid component.
/// </summary>
public class OrdersGridTests : TestContext
{
    private readonly List<Order> _testOrders;

    public OrdersGridTests()
    {
        _testOrders = new List<Order>
        {
            new() { OrderNumber = 1001, Status = OrderStatus.Placed, StoreId = "ST001", StoreName = "Downtown Store", NumberOfItems = 5, FulfillmentType = FulfillmentType.Pickup, FulfillmentPartner = "Mi9" },
            new() { OrderNumber = 1002, Status = OrderStatus.Completed, StoreId = "ST002", StoreName = "Mall Location", NumberOfItems = 3, FulfillmentType = FulfillmentType.Shipping, FulfillmentPartner = "FedEx" },
            new() { OrderNumber = 1003, Status = OrderStatus.Canceled, StoreId = "ST003", StoreName = "Airport Store", NumberOfItems = 8, FulfillmentType = FulfillmentType.Delivery, FulfillmentPartner = "Shipt" },
            new() { OrderNumber = 1004, Status = OrderStatus.Fulfillment, StoreId = "ST001", StoreName = "Downtown Store", NumberOfItems = 2, FulfillmentType = FulfillmentType.Pickup, FulfillmentPartner = "Mi9" }
        };
    }

    [Fact]
    public void OrdersGrid_RendersAllOrders()
    {
        // Act
        var component = RenderComponent<OrdersGrid>(parameters => parameters
            .Add(p => p.Orders, _testOrders));

        // Assert
        Assert.Contains("1001", component.Markup);
        Assert.Contains("1002", component.Markup);
        Assert.Contains("1003", component.Markup);
        Assert.Contains("1004", component.Markup);
    }

    [Fact]
    public void OrdersGrid_DisplaysCorrectOrderStatuses()
    {
        // Act
        var component = RenderComponent<OrdersGrid>(parameters => parameters
            .Add(p => p.Orders, _testOrders));

        // Assert
        Assert.Contains("Placed", component.Markup);
        Assert.Contains("Completed", component.Markup);
        Assert.Contains("Canceled", component.Markup);
        Assert.Contains("Fulfillment", component.Markup);
    }

    [Fact]
    public void OrdersGrid_HasSearchInput()
    {
        // Act
        var component = RenderComponent<OrdersGrid>(parameters => parameters
            .Add(p => p.Orders, _testOrders));

        // Assert
        var searchInput = component.Find("input[placeholder='Search orders...']");
        Assert.NotNull(searchInput);
    }

    [Fact]
    public void OrdersGrid_HasStatusFilterDropdown()
    {
        // Act
        var component = RenderComponent<OrdersGrid>(parameters => parameters
            .Add(p => p.Orders, _testOrders));

        // Assert
        var statusFilter = component.Find("select");
        Assert.NotNull(statusFilter);
        Assert.Contains("All Statuses", component.Markup);
        Assert.Contains("Placed", component.Markup);
        Assert.Contains("Fulfillment", component.Markup);
        Assert.Contains("Completed", component.Markup);
        Assert.Contains("Canceled", component.Markup);
    }

    [Fact]
    public void OrdersGrid_HasFulfillmentTypeFilterDropdown()
    {
        // Act
        var component = RenderComponent<OrdersGrid>(parameters => parameters
            .Add(p => p.Orders, _testOrders));

        // Assert
        var selects = component.FindAll("select");
        Assert.True(selects.Count >= 2);
        Assert.Contains("All Fulfillment Types", component.Markup);
        Assert.Contains("Pickup", component.Markup);
        Assert.Contains("Delivery", component.Markup);
        Assert.Contains("Shipping", component.Markup);
    }

    [Fact]
    public void OrdersGrid_ShowsActionsDropdownForActiveOrders()
    {
        // Act
        var component = RenderComponent<OrdersGrid>(parameters => parameters
            .Add(p => p.Orders, _testOrders));

        // Assert
        var actionsButtons = component.FindAll("button.dropdown-toggle");
        Assert.True(actionsButtons.Count >= 2); // For Placed and Fulfillment orders
        Assert.Contains("Actions", component.Markup);
    }

    [Fact]
    public void OrdersGrid_ShowsReopenButtonForCanceledOrders()
    {
        // Act
        var component = RenderComponent<OrdersGrid>(parameters => parameters
            .Add(p => p.Orders, _testOrders));

        // Assert
        var reopenButtons = component.FindAll("button").Where(b => b.TextContent.Contains("Reopen"));
        Assert.Single(reopenButtons); // Only one canceled order
    }

    [Fact]
    public void OrdersGrid_HidesActionsForCompletedOrders()
    {
        // Arrange
        var completedOrder = _testOrders.First(o => o.Status == OrderStatus.Completed);

        // Act
        var component = RenderComponent<OrdersGrid>(parameters => parameters
            .Add(p => p.Orders, _testOrders));

        // Assert
        var tableRows = component.FindAll("tbody tr");
        var completedOrderRow = tableRows.First(row => row.TextContent.Contains(completedOrder.OrderNumber.ToString()));
        var actionsCell = completedOrderRow.QuerySelector("td:last-child");
        
        // The actions cell should be mostly empty for completed orders
        Assert.DoesNotContain("Actions", actionsCell?.TextContent ?? "");
        Assert.DoesNotContain("Reopen", actionsCell?.TextContent ?? "");
    }

    [Fact]
    public void OrdersGrid_DisplaysCorrectStatusBadges()
    {
        // Act
        var component = RenderComponent<OrdersGrid>(parameters => parameters
            .Add(p => p.Orders, _testOrders));

        // Assert
        Assert.Contains("bg-primary", component.Markup); // Placed
        Assert.Contains("bg-success", component.Markup); // Completed
        Assert.Contains("bg-danger", component.Markup); // Canceled
        Assert.Contains("bg-warning", component.Markup); // Fulfillment
    }

    [Fact]
    public void OrdersGrid_HasSortableHeaders()
    {
        // Act
        var component = RenderComponent<OrdersGrid>(parameters => parameters
            .Add(p => p.Orders, _testOrders));

        // Assert
        var sortableHeaders = component.FindAll("th[style*='cursor: pointer']");
        Assert.True(sortableHeaders.Count >= 6); // Order Number, Status, Store ID, Store Name, Items, Fulfillment Type
    }

    [Fact]
    public void OrdersGrid_ShowsNoOrdersMessageWhenEmpty()
    {
        // Act
        var component = RenderComponent<OrdersGrid>(parameters => parameters
            .Add(p => p.Orders, new List<Order>()));

        // Assert
        Assert.Contains("No orders found matching your criteria", component.Markup);
    }

    [Fact]
    public void OrdersGrid_DisplaysCorrectFulfillmentPartners()
    {
        // Act
        var component = RenderComponent<OrdersGrid>(parameters => parameters
            .Add(p => p.Orders, _testOrders));

        // Assert
        Assert.Contains("Mi9", component.Markup);
        Assert.Contains("FedEx", component.Markup);
        Assert.Contains("Shipt", component.Markup);
    }

    [Fact]
    public void OrdersGrid_HasCorrectTableStructure()
    {
        // Act
        var component = RenderComponent<OrdersGrid>(parameters => parameters
            .Add(p => p.Orders, _testOrders));

        // Assert
        var table = component.Find("table");
        Assert.NotNull(table);
        Assert.Contains("table-striped", table.GetAttribute("class") ?? "");
        Assert.Contains("table-hover", table.GetAttribute("class") ?? "");

        var thead = component.Find("thead");
        Assert.NotNull(thead);
        Assert.Contains("table-dark", thead.GetAttribute("class") ?? "");

        var tbody = component.Find("tbody");
        Assert.NotNull(tbody);
    }

    [Fact]
    public void OrdersGrid_DisplaysAllRequiredColumns()
    {
        // Act
        var component = RenderComponent<OrdersGrid>(parameters => parameters
            .Add(p => p.Orders, _testOrders));

        // Assert
        var headers = component.FindAll("th");
        var headerTexts = headers.Select(h => h.TextContent.Trim()).ToList();

        Assert.Contains("Order Number", headerTexts[0]);
        Assert.Contains("Status", headerTexts[1]);
        Assert.Contains("Store ID", headerTexts[2]);
        Assert.Contains("Store Name", headerTexts[3]);
        Assert.Contains("Items", headerTexts[4]);
        Assert.Contains("Fulfillment Type", headerTexts[5]);
        Assert.Equal("Fulfillment Partner", headerTexts[6]);
        Assert.Equal("Actions", headerTexts[7]);
    }
}