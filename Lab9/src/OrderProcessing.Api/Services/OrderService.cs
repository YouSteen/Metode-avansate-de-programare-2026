using OrderProcessing.Api.Contracts;
using OrderProcessing.Api.Domain;
using OrderProcessing.Api.Validation;

namespace OrderProcessing.Api.Services;

public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IOrderValidationHandler _validationChain;

    public OrderService(IOrderRepository repository, IOrderValidationHandler validationChain)
    {
        _repository = repository;
        _validationChain = validationChain;
    }

    public (Order? Order, ValidationResult? Validation) CreateOrder(CreateOrderRequest request)
    {
        var items = request.Items.Select(i => new OrderItem(
            i.ProductId, i.ProductName, i.Quantity, i.UnitPrice, i.HasAgeRestriction)).ToList();

        var context = new ValidationContext
        {
            Customer = new Customer(
                request.Customer.Id,
                request.Customer.Name,
                request.Customer.Email,
                request.Customer.Age,
                request.Customer.IsTrusted),
            ShippingAddress = new Address(
                request.ShippingAddress.Street,
                request.ShippingAddress.City,
                request.ShippingAddress.PostalCode,
                request.ShippingAddress.Country),
            Items = items,
            DeclaredTotal = request.DeclaredTotal
        };

        var validation = _validationChain.Handle(context);
        if (!validation.IsValid)
            return (null, validation);

        var total = new Money(items.Sum(i => i.Quantity * i.UnitPrice));
        var order = new Order
        {
            Customer = context.Customer,
            ShippingAddress = context.ShippingAddress,
            Items = items,
            Total = total
        };

        _repository.Add(order);
        return (order, null);
    }

    public Order? GetOrder(OrderId id) => _repository.GetById(id);

    public IReadOnlyList<Order> GetAllOrders() => _repository.GetAll();

    public Order PayOrder(OrderId id) => Transition(id, o => o.Pay());

    public Order ProcessOrder(OrderId id) => Transition(id, o => o.Process());

    public Order ShipOrder(OrderId id) => Transition(id, o => o.Ship());

    public Order DeliverOrder(OrderId id) => Transition(id, o => o.Deliver());

    public Order CancelOrder(OrderId id) => Transition(id, o => o.Cancel());

    private Order Transition(OrderId id, Action<Order> action)
    {
        var order = _repository.GetById(id)
            ?? throw new KeyNotFoundException("Comanda nu exista");
        action(order);
        _repository.Update(order);
        return order;
    }
}
