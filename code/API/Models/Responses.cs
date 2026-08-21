using Application.Dtos;

namespace API.Models;

// Обертки чтобы можно было безболезненно добавлять дополнительные поля
// Например число записей в GetListResponse или параметры пагинации и тд
public record GetByIdResponse
{
    public required OpportunityDto Opportunity { get; init; }
}

public record GetListResponse
{
    public required IReadOnlyList<OpportunityDto> Opportunities { get; init; }
}

public record CreateResponse
{
    public Guid CreatedOpportunityId { get; init; }
}

public record AddItemResponse
{
    public Guid AddedItemId { get; init; }
}