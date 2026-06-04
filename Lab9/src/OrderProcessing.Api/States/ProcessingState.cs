using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.States;

public class ProcessingState : IOrderState
{
    public string Name => "Processing";

    public void Pay(Order order) => Throw(order, nameof(Pay));
    public void Process(Order order) => Throw(order, nameof(Process));
    public void Ship(Order order) => order.SetState(new ShippedState());
    public void Deliver(Order order) => Throw(order, nameof(Deliver));
    public void Cancel(Order order) => order.SetState(new CancelledState());

    private static void Throw(Order order, string action) =>
        throw new InvalidOrderTransitionException(action, order.Status);
}
