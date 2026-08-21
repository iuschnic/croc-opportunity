using Application.Dtos;
using Domain.Models;

namespace Application.Mappers;

public static class OpportunityItemMapper
{
    public static OpportunityItemDto ToDto(this OpportunityItem item) => new()
    {
        Id = item.Id,
        Name = item.Name,
        Quantity = item.Quantity,
        PricePerUnit = item.PricePerUnit,
        Discount = item.Discount,
    };

    public static IReadOnlyList<OpportunityItemDto> ToDto(this IEnumerable<OpportunityItem> items)
        => items.Select(i => i.ToDto()).ToList();
}