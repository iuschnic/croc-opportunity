using Application.Enums;

namespace Application.Commands;

public record CreateOpportunityCommand
{
    public Guid ContactId { get; init; }
    public CurrencyApp Currency { get; init; }
}