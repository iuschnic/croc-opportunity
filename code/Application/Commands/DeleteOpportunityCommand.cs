namespace Application.Commands;

public record DeleteOpportunityCommand
{
    public Guid Id { get; init; }
}