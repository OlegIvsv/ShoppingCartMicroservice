using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Common
{
    public abstract class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
    {
        public abstract IEnumerable<object> AtomicValuesList();

        public bool Equals(T? other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return AtomicValuesList().SequenceEqual(
                other.AtomicValuesList());
        }

        public override bool Equals(object? obj)
            => Equals(obj as T);

        public override int GetHashCode()
        {
            return AtomicValuesList()
                .Select(value => value != null ? value.GetHashCode() : 0)
                .Aggregate((total, next) => total ^ next);
        }

        public static bool operator ==(ValueObject<T>? left, ValueObject<T>? right)
            => Equals(left, right);

        public static bool operator !=(ValueObject<T>? left, ValueObject<T>? right)
            => !Equals(left, right);
    }
}
