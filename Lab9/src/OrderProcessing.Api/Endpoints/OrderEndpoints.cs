using OrderProcessing.Api.Contracts;
using OrderProcessing.Api.Domain;
using OrderProcessing.Api.Services;
using OrderProcessing.Api.States;

namespace OrderProcessing.Api.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/orders");

        group.MapPost("/", CreateOrder);
        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById);
        group.MapPost("/{id:guid}/pay", Pay);
        group.MapPost("/{id:guid}/process", Process);
        group.MapPost("/{id:guid}/ship", Ship);
        group.MapPost("/{id:guid}/deliver", Deliver);
        group.MapPost("/{id:guid}/cancel", Cancel);

        app.MapGet("/api/products", () => ProductCatalogDto.GetAll());
    }

    private static IResult CreateOrder(CreateOrderRequest request, OrderService service)
    {
        var (order, validation) = service.CreateOrder(request);
        if (validation is not null)
            return Results.BadRequest(new { errors = validation.Errors });

        return Results.Created($"/orders/{order!.Id.Value}", order);
    }

    private static IResult GetAll(OrderService service) =>
        Results.Ok(service.GetAllOrders());

    private static IResult GetById(Guid id, OrderService service)
    {
        var order = service.GetOrder(new OrderId(id));
        return order is null ? Results.NotFound() : Results.Ok(order);
    }

    private static IResult Pay(Guid id, OrderService service) => Transition(id, service.PayOrder);
    private static IResult Process(Guid id, OrderService service) => Transition(id, service.ProcessOrder);
    private static IResult Ship(Guid id, OrderService service) => Transition(id, service.ShipOrder);
    private static IResult Deliver(Guid id, OrderService service) => Transition(id, service.DeliverOrder);
    private static IResult Cancel(Guid id, OrderService service) => Transition(id, service.CancelOrder);

    private static IResult Transition(Guid id, Func<OrderId, Order> action)
    {
        try
        {
            var order = action(new OrderId(id));
            return Results.Ok(order);
        }
        catch (KeyNotFoundException)
        {
            return Results.NotFound(new { message = "Comanda nu exista" });
        }
        catch (InvalidOrderTransitionException ex)
        {
            return Results.Conflict(new { message = ex.Message });
        }
    }
}

internal static class ProductCatalogDto
{
    private static readonly Guid WineId = Guid.Parse("11111111-1111-1111-1111-111111111103");

    public static object GetAll() =>
        Validation.ProductStock.Names.Select(kv => new
        {
            productId = kv.Key,
            name = kv.Value,
            stock = Validation.ProductStock.Levels[kv.Key],
            unitPrice = kv.Key switch
            {
                var g when g == Guid.Parse("11111111-1111-1111-1111-111111111101") => 3500m,
                var g when g == Guid.Parse("11111111-1111-1111-1111-111111111102") => 89m,
                var g when g == WineId => 45m,
                _ => 120m
            },
            hasAgeRestriction = kv.Key == WineId
        });
}
