using System.ComponentModel.DataAnnotations;
using Application.Enums;

namespace Application.Dtos;

public record OpportunityDto
{
    public Guid Id {get; init;}
    public Guid ContactId {get; init;}
    public OpportunityStatusApp Status {get; init;}
    public required MoneyDto TotalAmount {get; init;}
    public DateTime CreatedAt {get; init;}
    public string? LossReason {get; init;}
    public IReadOnlyList<OpportunityItemDto> Items { get; init; } = [];
}