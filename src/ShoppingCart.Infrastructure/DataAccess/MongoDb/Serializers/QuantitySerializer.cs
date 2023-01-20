using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using ShoppingCart.Domain.ValueObjects;

namespace ShoppingCart.Infrastructure.DataAccess.MongoDb.Serializers;

internal class QuantitySerializer : SerializerBase<Quantity>
{
    public override void Serialize(
        BsonSerializationContext context,
        BsonSerializationArgs args,
        Quantity value)
    {
        context.Writer.WriteInt32(value.Value);
    }

    public override Quantity Deserialize(
        BsonDeserializationContext context,
        BsonDeserializationArgs args)
    {
        return Quantity.Create(context.Reader.ReadInt32()).Value;
    }
}