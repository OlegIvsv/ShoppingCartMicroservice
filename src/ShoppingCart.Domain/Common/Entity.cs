namespace ShoppingCart.Domain.Common;

public abstract class Entity<T> : IEquatable<T> where T : Entity<T>
{
    public Guid Id { get; protected init; }

    public bool Equals(T? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Id == other.Id;
    }

    public override bool Equals(object obj)
        => Equals(obj as Entity<T>);

    public override int GetHashCode()
        => Id.GetHashCode();

    public static bool operator ==(Entity<T> left, Entity<T> right)
        => Equals(left, right);

    public static bool operator !=(Entity<T> left, Entity<T> right)
        => !Equals(left, right);
}