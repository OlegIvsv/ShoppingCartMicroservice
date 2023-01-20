using FluentResults;

namespace ShoppingCart.Domain.Errors;

public class InvalidQuantityValueError : Error
{
    public InvalidQuantityValueError(int value)
        : base($"Invalid product quantity value : {value}")
    {
    }
}