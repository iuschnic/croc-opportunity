using Application.Enums;

namespace API.Models;

public record CreateOpportunityRequest
{
    public Guid ContactId { get; init; }
    public CurrencyApp Currency { get; init; }
}

public record UpdateOpportunityStatusRequest
{
    //public Guid Id { get; init; }
    public OpportunityStatusApp NewStatus { get; init; }
    public string? NewLossReason { get; init; }
}

public record UpdateOpportunityLossReasonRequest
{
    //public Guid Id { get; init; }
    public required string NewLossReason { get; init; }
}

public record AddOpportunityItemRequest
{
    //public Guid OpportunityId { get; init; }
    public required string Name { get; init; }
    public int Quantity {get; init;}
    public decimal PricePerUnit { get; init; }
    public decimal Discount {get; init;}
}

public record UpdateOpportunityItemRequest
{
    //public Guid OpportunityId { get; init; }
    //public Guid OpportunityItemId { get; init; }
    public required string NewName { get; init; }
    public int NewQuantity {get; init;}
    public decimal NewPricePerUnit { get; init; }
    public decimal NewDiscount {get; init;}
}