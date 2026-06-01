namespace OrderManagement.After.Models;

public class Order
{
    public Guid Id { get; set; }
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public List<OrderItem> Items { get; set; } = new();
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }

    public bool CanBeCancelled() => Status != OrderStatus.Shipped;

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Doar comenzile in asteptare pot fi confirmate");
        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        if (!CanBeCancelled())
            throw new InvalidOperationException("Comanda expediata nu poate fi anulata");
        Status = OrderStatus.Cancelled;
    }
}
