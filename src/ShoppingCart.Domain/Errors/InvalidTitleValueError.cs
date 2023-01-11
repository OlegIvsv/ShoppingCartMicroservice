using FluentResults;

namespace ShoppingCart.Domain.Errors
{
    public class InvalidTitleValueError : Error
    {
        public InvalidTitleValueError(string value)
            : base($"Invalid product title value {value}") { }
    }
}
