using FluentResults;

namespace ShoppingCart.Domain.Errors;

public class InvalidDiscountValueError : Error
{
    public InvalidDiscountValueError(double value)
        : base($"Invalid dicount value : {value}")
    {
    }
}