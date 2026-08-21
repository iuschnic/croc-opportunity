namespace Application.Dtos;

public record OpportunityItemDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public int Quantity { get; init; }
    public decimal PricePerUnit { get; init; }
    public decimal Discount { get; init; }
}