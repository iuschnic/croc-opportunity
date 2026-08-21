using Application.Dtos;
using Domain.Enums;
using Domain.Models;
using Storage.Models;
using Storage.Enums;

namespace Storage.Mappers;

public static class OpportunityMapper
{
    public static OpportunityDb ToDb(this Opportunity opportunity) => new()
    {
        Id = opportunity.Id,
        ContactId = opportunity.ContactId,
        Status = opportunity.Status.ToDb(),
        CreatedAt = opportunity.CreatedAt,
        LossReason =  opportunity.LossReason,
        Items = opportunity.Items.Select(i => i.ToDb(opportunity.Id)).ToList(),
        Currency = opportunity.Currency.ToDb()
    };
    
    public static Opportunity ToDomain(this OpportunityDb opportunity)
    {
        return Opportunity.Restore(opportunity.Id, opportunity.ContactId, 
            opportunity.Status.ToDomain(), opportunity.Currency.ToDomain(), opportunity.CreatedAt, 
            opportunity.Items.ToDomain(), opportunity.LossReason);
    }
    
    public static OpportunityDto ToDto(this OpportunityDb opportunity) => new()
    {
        Id = opportunity.Id,
        ContactId = opportunity.ContactId,
        Status = opportunity.Status.ToApp(),
        TotalAmount = new MoneyDto()
        {
            // дублирование логики из доменной сущности но это цена обхода доменной сущности
            Amount = opportunity.Items.Sum(item => item.PricePerUnit * item.Quantity * (1.0m - item.Discount)),
            Currency = opportunity.Currency.ToApp()
        },
        CreatedAt = opportunity.CreatedAt,
        LossReason =  opportunity.LossReason,
        Items = opportunity.Items.ToDto()
    };
    
    public static IReadOnlyList<Opportunity> ToDomain(this IEnumerable<OpportunityDb> opportunities)
        => opportunities.Select(o => o.ToDomain()).ToList();

    public static IReadOnlyList<OpportunityDto> ToDto(this IEnumerable<OpportunityDb> opportunities)
        => opportunities.Select(o => o.ToDto()).ToList();
}