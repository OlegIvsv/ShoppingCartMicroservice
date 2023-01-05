using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using ShoppingCart.Domain.Models;
using ShoppingCart.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure.DataAccess.MongoDb.Serrializers
{
    public class MoneySerrializer : SerializerBase<Money>
    {
        public override void Serialize(
            BsonSerializationContext context,
            BsonSerializationArgs args,
            Money value)
        {
            context.Writer.WriteDecimal128(value.Value);
        }

        public override Money Deserialize(
            BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            return Money.Create(
                BsonSerializer.Deserialize<decimal>(context.Reader)).Value;
        }
    }
}
