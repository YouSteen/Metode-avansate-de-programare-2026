namespace OrderProcessing.Api.Validation;

public static class ProductStock
{
    public static readonly IReadOnlyDictionary<Guid, int> Levels = new Dictionary<Guid, int>
    {
        [Guid.Parse("11111111-1111-1111-1111-111111111101")] = 50,
        [Guid.Parse("11111111-1111-1111-1111-111111111102")] = 30,
        [Guid.Parse("11111111-1111-1111-1111-111111111103")] = 0,
        [Guid.Parse("11111111-1111-1111-1111-111111111104")] = 100
    };

    public static readonly IReadOnlyDictionary<Guid, string> Names = new Dictionary<Guid, string>
    {
        [Guid.Parse("11111111-1111-1111-1111-111111111101")] = "Laptop",
        [Guid.Parse("11111111-1111-1111-1111-111111111102")] = "Mouse",
        [Guid.Parse("11111111-1111-1111-1111-111111111103")] = "Vin rosu",
        [Guid.Parse("11111111-1111-1111-1111-111111111104")] = "Carte tehnica"
    };
}
