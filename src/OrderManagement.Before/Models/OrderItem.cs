namespace OrderManagement.Before.Models;

public class OrderItem
{
    public string ProductSku { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
