using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Models;

public readonly record struct Money
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    public Money(decimal amount, Currency currency)
    {
        // В текущей модели не должно быть отрицательных цен/стоимостей
        if (amount < 0)
            throw new DomainValidationException("Money amount should never be less than zero");
        Amount = amount;
        Currency = currency;
    }

    public static Money Zero(Currency currency)
    {
        return new Money(0, currency);
    }
    
    public Money Zero()
    {
        return new Money(0, Currency);
    }

    public Money Add(decimal amount)
    {
        return new Money(Amount + amount, Currency);
    }
    
    public Money Subtract(decimal amount)
    {
        CheckIfCanSubstract(amount);
        return new Money(Amount - amount, Currency);
    }
    
    public Money Add(Money money)
    {
        CheckCurrencyMatch(money.Currency);
        return new Money(Amount + money.Amount, Currency);
    }

    public Money Subtract(Money money)
    {
        CheckCurrencyMatch(money.Currency);
        CheckIfCanSubstract(money.Amount);
        return new Money(Amount - money.Amount, Currency);
    }
    
    public static Money operator+(Money money, decimal amount)
    {
        return money.Add(amount);
    }
    
    public static Money operator+(decimal amount, Money money)
    {
        return money.Add(amount);
    }
    
    public static Money operator-(Money money, decimal amount)
    {
        return money.Subtract(amount);
    }
    
    public static Money operator-(decimal amount, Money money)
    {
        return money.Subtract(amount);
    }

    public static Money operator+(Money a, Money b)
    {
        return a.Add(b);
    }
    
    public static Money operator-(Money a, Money b)
    {
        return a.Subtract(b);
    }

    private void CheckIfCanSubstract(decimal amount)
    {
        if (Amount - amount < 0)
            throw new DomainValidationException("To subtract money the result should never be less than zero");
    }
    
    private void CheckCurrencyMatch(Currency currency)
    {
        if (currency != Currency)
            throw new DomainValidationException("Currencies should match when subtracting or adding money");
    }
}