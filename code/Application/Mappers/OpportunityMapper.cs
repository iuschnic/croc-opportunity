using Application.Dtos;
using Domain.Models;

namespace Application.Mappers;

public static class OpportunityMapper
{
    public static OpportunityDto ToDto(this Opportunity opportunity) => new()
    {
        Id = opportunity.Id,
        ContactId = opportunity.ContactId,
        Status = opportunity.Status.ToApp(),
        TotalAmount = opportunity.TotalAmount.ToDto(),
        CreatedAt = opportunity.CreatedAt,
        LossReason =  opportunity.LossReason,
        Items = opportunity.Items.ToDto()
    };

    public static IReadOnlyList<OpportunityDto> ToDto(this IEnumerable<Opportunity> opportunities)
        => opportunities.Select(o => o.ToDto()).ToList();
}