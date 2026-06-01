using OrderManagement.Before.Models;

namespace OrderManagement.Before;

/// <summary>
/// God Class: validare, stoc, plata, persistare SQL, email SMTP si rapoarte intr-o singura clasa.
/// Cuplare mare: tipuri concrete de infrastructura, fara abstractii.
/// </summary>
public class OrderManager
{
    private readonly string _sqlConnectionString = "Server=localhost;Database=OrdersDb;";
    private readonly string _smtpHost = "smtp.company.local";
    private readonly string _paymentApiUrl = "https://payments.company.local/charge";

    private readonly Dictionary<Guid, Order> _orders = new();
    private readonly Dictionary<string, int> _stock = new()
    {
        ["SKU-001"] = 50,
        ["SKU-002"] = 10,
        ["SKU-003"] = 0
    };

    public Guid PlaceOrder(string customerName, string customerEmail, List<OrderItem> items)
    {
        if (string.IsNullOrWhiteSpace(customerEmail))
            throw new ArgumentException("Email invalid");

        foreach (var item in items)
        {
            if (!_stock.TryGetValue(item.ProductSku, out var available) || available < item.Quantity)
                throw new InvalidOperationException($"Stoc insuficient pentru {item.ProductSku}");
        }

        var total = items.Sum(i => i.UnitPrice * i.Quantity);

        var paymentOk = ProcessPaymentInternal(customerEmail, total);
        if (!paymentOk)
            throw new InvalidOperationException("Plata a esuat");

        foreach (var item in items)
            _stock[item.ProductSku] -= item.Quantity;

        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerName = customerName,
            CustomerEmail = customerEmail,
            Items = items,
            TotalAmount = total,
            Status = OrderStatus.Confirmed,
            CreatedAt = DateTime.UtcNow
        };

        SaveOrderToDatabase(order);
        _orders[order.Id] = order;
        SendConfirmationEmail(order);
        return order.Id;
    }

    public bool CancelOrder(Guid orderId)
    {
        if (!_orders.TryGetValue(orderId, out var order))
            throw new InvalidOperationException("Comanda nu exista");

        if (order.Status == OrderStatus.Shipped)
            return false;

        order.Status = OrderStatus.Cancelled;
        SaveOrderToDatabase(order);

        foreach (var item in order.Items)
            _stock[item.ProductSku] += item.Quantity;

        SendCancellationEmail(order);
        return true;
    }

    public Order? GetOrder(Guid orderId) =>
        _orders.TryGetValue(orderId, out var order) ? order : null;

    public void UpdateOrderStatus(Guid orderId, OrderStatus newStatus)
    {
        if (!_orders.TryGetValue(orderId, out var order))
            throw new InvalidOperationException("Comanda nu exista");

        order.Status = newStatus;
        SaveOrderToDatabase(order);
    }

    public string GenerateSalesReport()
    {
        var confirmed = _orders.Values.Where(o => o.Status == OrderStatus.Confirmed).ToList();
        var revenue = confirmed.Sum(o => o.TotalAmount);
        return $"Raport vanzari: {confirmed.Count} comenzi, venit total {revenue:C}";
    }

    private bool ProcessPaymentInternal(string email, decimal amount)
    {
        Console.WriteLine($"[Payment API {_paymentApiUrl}] Charge {amount:C} pentru {email}");
        return amount > 0;
    }

    private void SaveOrderToDatabase(Order order)
    {
        Console.WriteLine($"[SQL {_sqlConnectionString}] SAVE Order {order.Id} status={order.Status}");
    }

    private void SendConfirmationEmail(Order order)
    {
        Console.WriteLine($"[SMTP {_smtpHost}] Confirmare comanda {order.Id} catre {order.CustomerEmail}");
    }

    private void SendCancellationEmail(Order order)
    {
        Console.WriteLine($"[SMTP {_smtpHost}] Anulare comanda {order.Id} catre {order.CustomerEmail}");
    }
}
