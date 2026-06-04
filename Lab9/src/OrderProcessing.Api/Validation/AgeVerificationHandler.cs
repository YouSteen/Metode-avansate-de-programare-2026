namespace OrderProcessing.Api.Validation;

public class AgeVerificationHandler : BaseValidationHandler
{
    protected override ValidationResult Validate(ValidationContext context)
    {
        var needsAge = context.Items.Any(i => i.HasAgeRestriction);
        if (needsAge && context.Customer.Age < 18)
            return ValidationResult.Failed("Clientul trebuie sa aiba minim 18 ani pentru produse cu restrictie");

        return ValidationResult.Ok();
    }
}
