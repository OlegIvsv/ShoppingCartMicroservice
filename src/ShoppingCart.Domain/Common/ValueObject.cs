namespace ShoppingCart.Domain.Common;

public abstract class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
{
    protected abstract IEnumerable<object> AtomicValuesList();

    public bool Equals(T? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return AtomicValuesList().SequenceEqual(
            other.AtomicValuesList());
    }
    
    public override int GetHashCode()
    {
        return AtomicValuesList()
            .Select(value => value is null ? 0 : value.GetHashCode())
            .Aggregate((total, next) => total ^ next);
    }
    
    public override bool Equals(object? obj)
        =>  Equals(obj as T);

    public static bool operator ==(ValueObject<T>? left, ValueObject<T>? right)
        => Equals(left, right);

    public static bool operator !=(ValueObject<T>? left, ValueObject<T>? right)
        => !Equals(left, right);
}