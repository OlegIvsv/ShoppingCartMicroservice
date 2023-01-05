using FluentResults;
using ShoppingCart.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.ValueObjects
{
    public sealed class Money : ValueObject<Money>
    {
        public const decimal MinMoneyValue = 0.01m;
        public decimal Value { get; private init; }

        private Money(decimal value)
        {
            Value = value;
        }

        public static Result<Money> Create(decimal value)
        {
            if (value < MinMoneyValue)
                return Result.Fail("Invalid price value");
            return new Money(value);
        }

        public override IEnumerable<object> AtomicValuesList()
        {
            yield return Value;
        }
    }
}
