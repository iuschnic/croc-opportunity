namespace Application.Commands;

public record AddOpportunityItemCommand
{
    public Guid OpportunityId { get; init; }
    public required string Name { get; init; }
    public int Quantity {get; init;}
    public decimal PricePerUnit { get; init; }
    public decimal Discount {get; init;}
}