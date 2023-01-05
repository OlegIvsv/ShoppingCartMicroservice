using FluentResults;
using ShoppingCart.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.ValueObjects
{
    public sealed class ProductTitle : ValueObject<ProductTitle>
    {
        public const int MaxTitleLength = 75;
        public string Value { get; private init; }

        private ProductTitle(string value)
        {
            Value = value;
        }

        public static Result<ProductTitle> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > MaxTitleLength)
                return Result.Fail("Invalid product title");
            return new ProductTitle(value);
        }

        public override IEnumerable<object> AtomicValuesList()
        {
            yield return Value;
        }
    }
}
