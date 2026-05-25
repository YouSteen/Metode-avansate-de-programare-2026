using OrderManagement.Before;
using OrderManagement.Before.Models;

var controller = new OrderController();

try
{
    var orderId = controller.PostOrder(
        "Ion Popescu",
        "ion@example.com",
        new List<OrderItem>
        {
            new() { ProductSku = "SKU-001", Quantity = 2, UnitPrice = 49.99m }
        });

    Console.WriteLine($"Comanda plasata: {orderId}");

    var cancelled = controller.DeleteOrder(orderId);
    Console.WriteLine($"Anulare reusita: {cancelled}");
}
catch (Exception ex)
{
    Console.WriteLine($"Eroare: {ex.Message}");
}
