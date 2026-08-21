using Application.Enums;
using Domain.Enums;

namespace Application.Mappers;

public static class ExpenseCategoryMapper
{
    public static CurrencyApp ToApp(this Currency currency) => currency switch
    {
        Currency.RUB => CurrencyApp.RUB,
        Currency.USD => CurrencyApp.USD,
        Currency.CNY => CurrencyApp.CNY,
        Currency.EUR => CurrencyApp.EUR,
        _ => throw new ArgumentOutOfRangeException(
            nameof(currency), currency, "Unknown currency"),
    };
    public static Currency ToDomain(this CurrencyApp currency) => currency switch
    {
        CurrencyApp.RUB => Currency.RUB,
        CurrencyApp.USD => Currency.USD,
        CurrencyApp.CNY => Currency.CNY,
        CurrencyApp.EUR => Currency.EUR,
        _ => throw new ArgumentOutOfRangeException(
            nameof(currency), currency, "Unknown currency"),
    };
}