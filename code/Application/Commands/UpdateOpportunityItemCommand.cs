namespace Application.Commands;

public record UpdateOpportunityItemCommand
{
    public Guid OpportunityId { get; init; }
    public Guid OpportunityItemId { get; init; }
    public required string NewName { get; init; }
    public int NewQuantity {get; init;}
    public decimal NewPricePerUnit { get; init; }
    public decimal NewDiscount {get; init;}
}