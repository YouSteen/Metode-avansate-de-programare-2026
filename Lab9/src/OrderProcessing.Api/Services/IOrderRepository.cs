using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.Services;

public interface IOrderRepository
{
    void Add(Order order);
    void Update(Order order);
    Order? GetById(OrderId id);
    IReadOnlyList<Order> GetAll();
}
