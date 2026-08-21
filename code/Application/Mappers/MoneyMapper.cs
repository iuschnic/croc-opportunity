using Application.Dtos;
using Domain.Models;

namespace Application.Mappers;

public static class MoneyMapper
{
    public static MoneyDto ToDto(this Money money) => new()
    {
        Amount = money.Amount,
        Currency = money.Currency.ToApp()
    };
}