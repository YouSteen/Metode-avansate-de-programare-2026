namespace OrderProcessing.Api.Validation;

public class FraudDetectionHandler : BaseValidationHandler
{
    private const decimal MaxUntrustedAmount = 10000m;
    private const int MaxDistinctItems = 50;

    protected override ValidationResult Validate(ValidationContext context)
    {
        if (!context.Customer.IsTrusted && context.DeclaredTotal > MaxUntrustedAmount)
            return ValidationResult.Failed("Comanda depaseste 10000 RON pentru client ne-trusted");

        if (context.Items.Count > MaxDistinctItems)
            return ValidationResult.Failed("Prea multe linii distincte intr-o singura comanda");

        return ValidationResult.Ok();
    }
}
