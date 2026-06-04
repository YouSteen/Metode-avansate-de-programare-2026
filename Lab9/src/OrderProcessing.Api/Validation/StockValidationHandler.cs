namespace OrderProcessing.Api.Validation;

public class StockValidationHandler : BaseValidationHandler
{
    protected override ValidationResult Validate(ValidationContext context)
    {
        foreach (var item in context.Items)
        {
            if (!ProductStock.Levels.TryGetValue(item.ProductId, out var stock))
                return ValidationResult.Failed($"Produs necunoscut: {item.ProductId}");

            if (stock < item.Quantity)
                return ValidationResult.Failed($"Stoc insuficient pentru {item.ProductName}");
        }

        return ValidationResult.Ok();
    }
}
