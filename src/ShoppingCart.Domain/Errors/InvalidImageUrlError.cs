using FluentResults;

namespace ShoppingCart.Domain.Errors;

public class InvalidImageUrlError : Error
{
    public InvalidImageUrlError(string value)
        : base($"'{value}' is not valid image url")
    {
    }
}