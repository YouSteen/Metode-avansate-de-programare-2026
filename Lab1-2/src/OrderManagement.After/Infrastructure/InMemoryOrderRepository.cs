using OrderManagement.After.Abstractions;
using OrderManagement.After.Models;

namespace OrderManagement.After.Infrastructure;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly Dictionary<Guid, Order> _orders = new();

    public Order? FindById(Guid id) =>
        _orders.TryGetValue(id, out var order) ? order : null;

    public void Save(Order order) => _orders[order.Id] = order;
}
