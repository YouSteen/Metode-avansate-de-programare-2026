using OrderManagement.After.Abstractions;
using OrderManagement.After.Models;

namespace OrderManagement.After.Infrastructure;

public class SmtpEmailService : IEmailService
{
    public void SendOrderConfirmation(Order order) =>
        Console.WriteLine($"[SMTP] Confirmare comanda {order.Id} -> {order.CustomerEmail}");

    public void SendOrderCancellation(Order order) =>
        Console.WriteLine($"[SMTP] Anulare comanda {order.Id} -> {order.CustomerEmail}");
}
