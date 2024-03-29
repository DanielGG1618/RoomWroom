﻿using Domain.Common.Exceptions;

namespace Domain.Common.ValueObjects;

public class Money : ValueObjectBase
{
    public static Money Zero { get; } = new(0, Currency.None);
    
    public decimal Amount { get; private set; }
    public Currency Currency { get; private set; }
    
    public Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public Money ToRounded() => new(Math.Ceiling(Amount), Currency);

    public static Money operator *(Money left, decimal right) => new(left.Amount * right, left.Currency);
    public static Money operator *(decimal left, Money right) => new(right.Amount * left, right.Currency);

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new MismatchedСurrenciesExceptions();

        return new(left.Amount + right.Amount, left.Currency);
    }
    public static Money operator -(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new MismatchedСurrenciesExceptions();

        return new(left.Amount - right.Amount, left.Currency);
    }
    
    public static bool operator <(Money left, decimal right) => left.Amount < right;
    public static bool operator >(Money left, decimal right) => left.Amount > right;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
    
    public override string ToString() => $"{Amount} {Currency}";

    private Money()
    {
    }
}
