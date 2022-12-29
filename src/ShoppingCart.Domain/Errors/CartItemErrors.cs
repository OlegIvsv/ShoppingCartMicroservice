namespace ShoppingCart.Domain.Errors
{
    public static class CartItemErrors
    {
        public class PriceException : DomainException { };
        public class ProductTitleException : DomainException { }
        public class QuantityException : DomainException { }
    }
}
