namespace OrderProcessing.Api.Domain;

public record StateTransition(string FromState, string ToState, DateTime At);
