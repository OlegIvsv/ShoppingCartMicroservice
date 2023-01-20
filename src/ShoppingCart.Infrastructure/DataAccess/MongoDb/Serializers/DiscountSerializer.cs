using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using ShoppingCart.Domain.ValueObjects;

namespace ShoppingCart.Infrastructure.DataAccess.MongoDb.Serializers;

internal class DiscountSerializer : SerializerBase<Discount>
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