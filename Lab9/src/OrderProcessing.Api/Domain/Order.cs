using OrderProcessing.Api.States;

namespace OrderProcessing.Api.Domain;

public class Order
{
    private IOrderState _currentState = new PendingState();

    public OrderId Id { get; init; } = OrderId.New();
    public Customer Customer { get; init; } = null!;
    public Address ShippingAddress { get; init; } = null!;
    public List<OrderItem> Items { get; init; } = new();
    public Money Total { get; init; } = Money.Zero();
    public string Status => _currentState.Name;
    public List<StateTransition> History { get; } = new();

    public void Pay() => _currentState.Pay(this);
    public void Process() => _currentState.Process(this);
    public void Ship() => _currentState.Ship(this);
    public void Deliver() => _currentState.Deliver(this);
    public void Cancel() => _currentState.Cancel(this);

    internal void SetState(IOrderState newState)
    {
        var from = _currentState.Name;
        _currentState = newState;
        History.Add(new StateTransition(from, newState.Name, DateTime.UtcNow));
    }
}
