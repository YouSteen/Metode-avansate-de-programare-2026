namespace OrderProcessing.Api.Contracts;

public record CreateOrderItemRequest(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    bool HasAgeRestriction);

public record CreateCustomerRequest(
    Guid Id,
    string Name,
    string Email,
    int Age,
    bool IsTrusted);

public record CreateAddressRequest(
    string Street,
    string City,
    string PostalCode,
    string Country);

public record CreateOrderRequest(
    CreateCustomerRequest Customer,
    CreateAddressRequest ShippingAddress,
    IReadOnlyList<CreateOrderItemRequest> Items,
    decimal DeclaredTotal);
