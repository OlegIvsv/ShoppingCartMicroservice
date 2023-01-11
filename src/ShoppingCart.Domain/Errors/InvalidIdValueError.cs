using FluentResults;

namespace ShoppingCart.Domain.Errors
{
    public class InvalidIdValueError: Error
    {
        public InvalidIdValueError(Guid value)
            : base($"Invalid id value : {value}") { }
    }
}

