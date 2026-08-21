using Application.Enums;

namespace Application.Commands;

public record UpdateOpportunityStatusCommand
{
    public Guid Id { get; init; }
    public OpportunityStatusApp NewStatus { get; init; }
    public string? NewLossReason { get; init; }
}