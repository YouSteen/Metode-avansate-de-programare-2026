namespace OrderProcessing.Api.States;

public class InvalidOrderTransitionException : Exception
{
    public InvalidOrderTransitionException(string action, string currentState)
        : base($"Nu pot executa '{action}' in starea '{currentState}'")
    {
    }
}
