using FluentResults;

namespace ShoppingCart.Domain.Errors;

public class ValueNotProvidedError : Error
{
    public ValueNotProvidedError()
        : base($"Value was not provided")
    {
    }
}