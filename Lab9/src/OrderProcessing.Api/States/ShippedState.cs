using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.States;

public class ShippedState : IOrderState
{
    public string Name => "Shipped";

    public void Pay(Order order) => Throw(order, nameof(Pay));
    public void Process(Order order) => Throw(order, nameof(Process));
    public void Ship(Order order) => Throw(order, nameof(Ship));
    public void Deliver(Order order) => order.SetState(new DeliveredState());
    public void Cancel(Order order) => Throw(order, nameof(Cancel));

    private static void Throw(Order order, string action) =>
        throw new InvalidOrderTransitionException(action, order.Status);
}
