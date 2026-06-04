using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.Validation;

public class ValidationContext
{
    public Customer Customer { get; init; } = null!;
    public Address ShippingAddress { get; init; } = null!;
    public IReadOnlyList<OrderItem> Items { get; init; } = Array.Empty<OrderItem>();
    public decimal DeclaredTotal { get; init; }
}
