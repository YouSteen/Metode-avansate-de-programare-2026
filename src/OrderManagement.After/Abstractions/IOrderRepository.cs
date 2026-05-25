using OrderManagement.After.Models;

namespace OrderManagement.After.Abstractions;

public interface IOrderRepository
{
    Order? FindById(Guid id);
    void Save(Order order);
}
