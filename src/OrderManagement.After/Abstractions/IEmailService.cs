using OrderManagement.After.Models;

namespace OrderManagement.After.Abstractions;

public interface IEmailService
{
    void SendOrderConfirmation(Order order);
    void SendOrderCancellation(Order order);
}
