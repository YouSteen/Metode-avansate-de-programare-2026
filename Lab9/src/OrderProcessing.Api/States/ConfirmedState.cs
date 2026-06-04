using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.States;

public class ConfirmedState : IOrderState
{
    public string Name => "Confirmed";

    public void Pay(Order order) => Throw(order, nameof(Pay));
    public void Process(Order order) => order.SetState(new ProcessingState());
    public void Ship(Order order) => Throw(order, nameof(Ship));
    public void Deliver(Order order) => Throw(order, nameof(Deliver));
    public void Cancel(Order order) => order.SetState(new CancelledState());

    private static void Throw(Order order, string action) =>
        throw new InvalidOrderTransitionException(action, order.Status);
}
