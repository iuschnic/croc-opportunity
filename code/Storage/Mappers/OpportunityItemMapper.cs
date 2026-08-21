using Application.Dtos;
using Domain.Models;
using Storage.Models;

namespace Storage.Mappers;

public static class OpportunityItemMapper
{
    public static OpportunityItemDb ToDb(this OpportunityItem item, Guid opportunityId) => new()
    {
        Id = item.Id,
        OpportunityId = opportunityId,
        Name = item.Name,
        Quantity = item.Quantity,
        PricePerUnit = item.PricePerUnit,
        Discount = item.Discount,
    };
    
    public static OpportunityItem ToDomain(this OpportunityItemDb item)
    {
        return OpportunityItem.Restore(item.Id, item.Name, item.Quantity, 
            item.PricePerUnit, item.Discount);
    }
    
    public static OpportunityItemDto ToDto(this OpportunityItemDb item) => new()
    {
        Id = item.Id,
        Name = item.Name,
        Quantity = item.Quantity,
        PricePerUnit = item.PricePerUnit,
        Discount = item.Discount,
    };
    
    public static IReadOnlyList<OpportunityItemDb> ToDb(this IEnumerable<OpportunityItem> items, Guid opportunityId)
        => items.Select(i => i.ToDb(opportunityId)).ToList();

    public static IReadOnlyList<OpportunityItem> ToDomain(this IEnumerable<OpportunityItemDb> items)
        => items.Select(i => i.ToDomain()).ToList();
    
    public static IReadOnlyList<OpportunityItemDto> ToDto(this IEnumerable<OpportunityItemDb> items)
        => items.Select(i => i.ToDto()).ToList();
}