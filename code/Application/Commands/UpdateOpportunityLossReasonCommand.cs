using Application.Enums;

namespace Application.Commands;

public record UpdateOpportunityLossReasonCommand
{
    public Guid Id { get; init; }
    public required string NewLossReason { get; init; }
}