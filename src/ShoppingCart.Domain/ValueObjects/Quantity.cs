using FluentResults;
using ShoppingCart.Domain.Common;
using ShoppingCart.Domain.Errors;

namespace ShoppingCart.Domain.ValueObjects;

public sealed class Quantity : ValueObject<Quantity>
{
    private const int MaxQuantityForProduct = 100;
    private const int MinQuantityForProduct = 1;
    public int Value { get; }

    private Quantity(int value)
    {
        Value = value;
    }

    public static Result<Quantity> Create(int value)
    {
        if (value is > MaxQuantityForProduct or < MinQuantityForProduct)
            return Result.Fail(new InvalidQuantityValueError(value));
        return new Quantity(value);
    }

    public static Quantity Add(Quantity first, Quantity second)
    {
        return new Quantity(
            Math.Min(first.Value + second.Value, MaxQuantityForProduct));
    }

    protected override IEnumerable<object> AtomicValuesList()
    {
        yield return Value;
    }
}