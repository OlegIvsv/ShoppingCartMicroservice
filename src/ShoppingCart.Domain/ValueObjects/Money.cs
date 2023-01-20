using FluentResults;
using ShoppingCart.Domain.Common;
using ShoppingCart.Domain.Errors;

namespace ShoppingCart.Domain.ValueObjects;

public sealed class Money : ValueObject<Money>
{
    private const decimal MinMoneyValue = 0.01m;
    public decimal Value { get; }

    private Money(decimal value)
    {
        Value = value;
    }

    public static Result<Money> Create(decimal value)
    {
        if (value < MinMoneyValue)
            return Result.Fail(new InavalidMoneyValueError(value));
        return new Money(value);
    }

    protected override IEnumerable<object> AtomicValuesList()
    {
        yield return Value;
    }
}