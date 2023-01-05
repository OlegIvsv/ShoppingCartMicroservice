using FluentResults;
using ShoppingCart.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.ValueObjects
{
    public sealed class Quantity : ValueObject<Quantity>
    {

        public const int MaxQuantityForProduct = 100;
        public int Value { get; private init; }

        public Quantity(int value)
        {
            Value = value;
        }

        public static Result<Quantity> Create(int value)
        {
            if (value is > MaxQuantityForProduct or < 1)
                return Result.Fail("Invalid quantity value");
            return new Quantity(value);
        }

        public static Quantity Add(Quantity first, Quantity second)
        {
            return new Quantity(
                Math.Min(first.Value + second.Value, MaxQuantityForProduct));
        }
        
        public override IEnumerable<object> AtomicValuesList()
        {
            yield return Value;
        }
    }
}
