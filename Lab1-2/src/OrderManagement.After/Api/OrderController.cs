using OrderManagement.After.Models;
using OrderManagement.After.Services;

namespace OrderManagement.After.Api;

public class OrderController
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService) => _orderService = orderService;

    public Guid PostOrder(string customerName, string customerEmail, List<OrderItem> items) =>
        _orderService.PlaceOrder(customerName, customerEmail, items);

    public bool DeleteOrder(Guid orderId) =>
        _orderService.CancelOrder(orderId);

    public Order? GetOrder(Guid orderId) =>
        _orderService.GetOrder(orderId);
}
