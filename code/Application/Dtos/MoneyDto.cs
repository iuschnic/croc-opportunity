using Application.Enums;

namespace Application.Dtos;

public record MoneyDto
{
    public decimal Amount {get; init;}
    public CurrencyApp Currency {get; init;}
}