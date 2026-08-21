using Storage.Enums;
using Domain.Enums;
using Application.Enums;

namespace Storage.Mappers;

public static class ExpenseCategoryMapper
{
    public static CurrencyDb ToDb(this Currency currency) => currency switch
    {
        Currency.RUB => CurrencyDb.RUB,
        Currency.USD => CurrencyDb.USD,
        Currency.CNY => CurrencyDb.CNY,
        Currency.EUR => CurrencyDb.EUR,
        _ => throw new ArgumentOutOfRangeException(
            nameof(currency), currency, "Unknown currency"),
    };
    public static Currency ToDomain(this CurrencyDb currency) => currency switch
    {
        CurrencyDb.RUB => Currency.RUB,
        CurrencyDb.USD => Currency.USD,
        CurrencyDb.CNY => Currency.CNY,
        CurrencyDb.EUR => Currency.EUR,
        _ => throw new ArgumentOutOfRangeException(
            nameof(currency), currency, "Unknown currency"),
    };
    public static CurrencyApp ToApp(this CurrencyDb currency) => currency switch
    {
        CurrencyDb.RUB => CurrencyApp.RUB,
        CurrencyDb.USD => CurrencyApp.USD,
        CurrencyDb.CNY => CurrencyApp.CNY,
        CurrencyDb.EUR => CurrencyApp.EUR,
        _ => throw new ArgumentOutOfRangeException(
            nameof(currency), currency, "Unknown currency"),
    };
}