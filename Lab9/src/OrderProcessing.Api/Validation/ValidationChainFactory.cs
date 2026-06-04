namespace OrderProcessing.Api.Validation;

public static class ValidationChainFactory
{
    public static IOrderValidationHandler Build()
    {
        var stock = new StockValidationHandler();
        var price = new PriceValidationHandler();
        var fraud = new FraudDetectionHandler();
        var age = new AgeVerificationHandler();
        stock.SetNext(price).SetNext(fraud).SetNext(age);
        return stock;
    }
}
