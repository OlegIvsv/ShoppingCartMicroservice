using FluentResults;
using ShoppingCart.Domain.Common;
using ShoppingCart.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.ValueObjects
{
    public sealed class Discount : ValueObject<Discount>
    {
        public const double MinDiscount = 0.00;
        public const double MaxDiscount = 1.00;
        public double Value { get; private init; }
        public static Discount Zero => new Discount(0);

        private Discount(double value)
        {
            Value = value;
        }

        public static Result<Discount> Create(double value)
        {
            if (value is > MaxDiscount or < MinDiscount)
                return Result.Fail(new InvalidDiscountValueError(value));
            
            return new Discount(value);
        }

        public override IEnumerable<object> AtomicValuesList()
        {
            yield return Value;
        }
    }
}
