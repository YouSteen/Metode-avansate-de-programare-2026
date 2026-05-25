using OrderManagement.After.Api;
using OrderManagement.After.Infrastructure;
using OrderManagement.After.Models;
using OrderManagement.After.Services;

var orderService = new OrderService(
    new InMemoryOrderRepository(),
    new InMemoryStockService(),
    new PaymentGatewayService(),
    new SmtpEmailService());

var controller = new OrderController(orderService);

try
{
    var orderId = controller.PostOrder(
        "Maria Ionescu",
        "maria@example.com",
        new List<OrderItem>
        {
            new() { ProductSku = "SKU-001", Quantity = 1, UnitPrice = 29.99m }
        });

    Console.WriteLine($"Comanda plasata: {orderId}");

    var cancelled = controller.DeleteOrder(orderId);
    Console.WriteLine($"Anulare reusita: {cancelled}");
}
catch (Exception ex)
{
    Console.WriteLine($"Eroare: {ex.Message}");
}
