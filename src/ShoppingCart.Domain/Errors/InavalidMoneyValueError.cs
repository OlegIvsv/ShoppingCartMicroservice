using FluentResults;

namespace ShoppingCart.Domain.Errors
{
    public class InavalidMoneyValueError: Error
    {
        public InavalidMoneyValueError(decimal value)
            : base($"Invalid money value : {value}") { }
    }
}
