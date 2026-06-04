namespace OrderProcessing.Api.Validation;

public interface IOrderValidationHandler
{
    IOrderValidationHandler SetNext(IOrderValidationHandler handler);
    ValidationResult Handle(ValidationContext context);
}
