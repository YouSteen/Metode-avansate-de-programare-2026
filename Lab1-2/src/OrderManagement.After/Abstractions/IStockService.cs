using OrderManagement.After.Models;

namespace OrderManagement.After.Abstractions;

public interface IStockService
{
    bool HasStock(IReadOnlyList<OrderItem> items);
    void Reserve(IReadOnlyList<OrderItem> items);
    void Release(IReadOnlyList<OrderItem> items);
}
