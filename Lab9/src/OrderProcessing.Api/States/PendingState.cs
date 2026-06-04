using OrderProcessing.Api.Domain;

namespace OrderProcessing.Api.States;

public class PendingState : IOrderState
{
    public string Name => "Pending";

    public void Pay(Order order) => order.SetState(new ConfirmedState());
    public void Process(Order order) => Throw(order, nameof(Process));
    public void Ship(Order order) => Throw(order, nameof(Ship));
    public void Deliver(Order order) => Throw(order, nameof(Deliver));
    public void Cancel(Order order) => order.SetState(new CancelledState());

    private static void Throw(Order order, string action) =>
        throw new InvalidOrderTransitionException(action, order.Status);
}
