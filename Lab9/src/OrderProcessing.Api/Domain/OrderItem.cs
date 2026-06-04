namespace OrderProcessing.Api.Domain;

public record OrderItem(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, bool HasAgeRestriction);
