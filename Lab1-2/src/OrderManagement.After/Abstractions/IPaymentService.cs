namespace OrderManagement.After.Abstractions;

public interface IPaymentService
{
    PaymentResult ProcessPayment(string customerEmail, decimal amount);
}

public record PaymentResult(bool Success, string? TransactionId);
