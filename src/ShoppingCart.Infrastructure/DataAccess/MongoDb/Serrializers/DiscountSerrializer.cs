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
    public class DiscountSerrializer : SerializerBase<Discount>
    {
        public override void Serialize(
            BsonSerializationContext context,
            BsonSerializationArgs args,
            Discount value)
        {
            context.Writer.WriteDouble(value.Value);
        }

        public override Discount Deserialize(
            BsonDeserializationContext context,
            BsonDeserializationArgs args)
        {
            return Discount.Create(
                BsonSerializer.Deserialize<double>(context.Reader)).Value;
        }
    }
}
