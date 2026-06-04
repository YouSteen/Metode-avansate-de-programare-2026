namespace OrderProcessing.Api.Validation;

public class PriceValidationHandler : BaseValidationHandler
{
    protected override ValidationResult Validate(ValidationContext context)
    {
        decimal sum = 0;
        foreach (var item in context.Items)
        {
            if (item.UnitPrice <= 0)
                return ValidationResult.Failed($"Pret invalid pentru {item.ProductName}");

            sum += item.Quantity * item.UnitPrice;
        }

        if (Math.Abs(sum - context.DeclaredTotal) > 0.01m)
            return ValidationResult.Failed("Totalul declarat nu coincide cu suma liniilor");

        return ValidationResult.Ok();
    }
}
