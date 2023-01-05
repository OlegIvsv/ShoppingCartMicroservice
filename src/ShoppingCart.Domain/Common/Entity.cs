using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Common
{
    public abstract class Entity<T> : IEquatable<T> where T : Entity<T>
    {
        public virtual Guid Id { get; protected init; }


        public override bool Equals(object obj)
        {
            return Equals(obj as Entity<T>);
        }

        public bool Equals(T? other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }


        public static bool operator ==(Entity<T> left, Entity<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<T> left, Entity<T> right)
        {
            return !Equals(left, right);
        }
    }
}
