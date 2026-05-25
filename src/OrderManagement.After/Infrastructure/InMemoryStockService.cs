using OrderManagement.After.Abstractions;
using OrderManagement.After.Models;

namespace OrderManagement.After.Infrastructure;

public class InMemoryStockService : IStockService
{
    private readonly Dictionary<string, int> _stock = new()
    {
        ["SKU-001"] = 50,
        ["SKU-002"] = 10,
        ["SKU-003"] = 0
    };

    public bool HasStock(IReadOnlyList<OrderItem> items) =>
        items.All(i => _stock.TryGetValue(i.ProductSku, out var qty) && qty >= i.Quantity);

    public void Reserve(IReadOnlyList<OrderItem> items)
    {
        foreach (var item in items)
            _stock[item.ProductSku] -= item.Quantity;
    }

    public void Release(IReadOnlyList<OrderItem> items)
    {
        foreach (var item in items)
            _stock[item.ProductSku] += item.Quantity;
    }
}
