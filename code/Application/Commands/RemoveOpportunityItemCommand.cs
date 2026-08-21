namespace Application.Commands;

public record RemoveOpportunityItemCommand
{
    public Guid OpportunityId { get; init; }
    public Guid OpportunityItemId { get; init; }
}