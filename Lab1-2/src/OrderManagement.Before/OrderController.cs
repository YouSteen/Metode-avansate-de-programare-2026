using OrderManagement.Before.Models;

namespace OrderManagement.Before;

/// <summary>
/// Controller subtire care delegheaza tot catre God Class.
/// </summary>
public class OrderController
{
    private readonly OrderManager _manager = new();

    public Guid PostOrder(string customerName, string customerEmail, List<OrderItem> items) =>
        _manager.PlaceOrder(customerName, customerEmail, items);

    public bool DeleteOrder(Guid orderId) =>
        _manager.CancelOrder(orderId);

    public Order? GetOrder(Guid orderId) =>
        _manager.GetOrder(orderId);
}
