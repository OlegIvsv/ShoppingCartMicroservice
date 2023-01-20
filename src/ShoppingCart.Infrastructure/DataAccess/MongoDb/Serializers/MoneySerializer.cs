using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using ShoppingCart.Domain.ValueObjects;

namespace ShoppingCart.Infrastructure.DataAccess.MongoDb.Serializers;

internal class MoneySerializer : SerializerBase<Money>
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