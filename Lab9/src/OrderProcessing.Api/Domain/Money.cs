namespace OrderProcessing.Api.Domain;

public record Money(decimal Amount, string Currency = "RON")
{
    public static Money Zero(string currency = "RON") => new(0, currency);
}
