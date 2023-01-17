namespace ShoppingCart.Domain.Common;

public abstract class AggregateRoot<T> : Entity<T> where T : AggregateRoot<T>
{
}