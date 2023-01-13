using FluentResults;
using ShoppingCart.Domain.Common;
using ShoppingCart.Domain.Errors;

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
                return Result.Fail(new InvalidTitleValueError(value));
            return new ProductTitle(value);
        }

        protected override IEnumerable<object> AtomicValuesList()
        {
            yield return Value;
        }
    }
}
