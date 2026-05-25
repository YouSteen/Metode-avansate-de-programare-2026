using OrderManagement.After.Abstractions;
using OrderManagement.After.Models;

namespace OrderManagement.After.Services;

/// <summary>
/// Coeziune mare: orchestrare flux comenzi. Cuplare mica: depinde doar de interfete.
/// </summary>
public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IStockService _stockService;
    private readonly IPaymentService _paymentService;
    private readonly IEmailService _emailService;

    public OrderService(
        IOrderRepository repository,
        IStockService stockService,
        IPaymentService paymentService,
        IEmailService emailService)
    {
        _repository = repository;
        _stockService = stockService;
        _paymentService = paymentService;
        _emailService = emailService;
    }

    public Guid PlaceOrder(string customerName, string customerEmail, List<OrderItem> items)
    {
        if (string.IsNullOrWhiteSpace(customerEmail))
            throw new ArgumentException("Email invalid");

        if (!_stockService.HasStock(items))
            throw new InvalidOperationException("Stoc insuficient");

        var total = items.Sum(i => i.UnitPrice * i.Quantity);
        var payment = _paymentService.ProcessPayment(customerEmail, total);
        if (!payment.Success)
            throw new InvalidOperationException("Plata a esuat");

        _stockService.Reserve(items);

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerName = customerName,
            CustomerEmail = customerEmail,
            Items = items,
            TotalAmount = total,
            CreatedAt = DateTime.UtcNow
        };
        order.Confirm();

        _repository.Save(order);
        _emailService.SendOrderConfirmation(order);
        return order.Id;
    }

    public bool CancelOrder(Guid orderId)
    {
        var order = _repository.FindById(orderId)
            ?? throw new InvalidOperationException("Comanda nu exista");

        if (!order.CanBeCancelled())
            return false;

        order.Cancel();
        _stockService.Release(order.Items);
        _repository.Save(order);
        _emailService.SendOrderCancellation(order);
        return true;
    }

    public Order? GetOrder(Guid orderId) => _repository.FindById(orderId);
}
