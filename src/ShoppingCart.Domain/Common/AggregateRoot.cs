using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Common
{
    public abstract class AggregateRoot<T> : Entity<T> where T : AggregateRoot<T>
    {
    }
}
