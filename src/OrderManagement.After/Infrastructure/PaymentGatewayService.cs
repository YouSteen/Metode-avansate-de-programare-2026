using OrderManagement.After.Abstractions;

namespace OrderManagement.After.Infrastructure;

public class PaymentGatewayService : IPaymentService
{
    public PaymentResult ProcessPayment(string customerEmail, decimal amount)
    {
        Console.WriteLine($"[PaymentGateway] Charge {amount:C} pentru {customerEmail}");
        return amount > 0
            ? new PaymentResult(true, Guid.NewGuid().ToString("N")[..12])
            : new PaymentResult(false, null);
    }
}
