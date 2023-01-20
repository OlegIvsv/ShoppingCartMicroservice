using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using ShoppingCart.Domain.ValueObjects;

namespace ShoppingCart.Infrastructure.DataAccess.MongoDb.Serializers;

internal class ProductTitleSerializer : SerializerBase<ProductTitle>
{
    public override void Serialize(
        BsonSerializationContext context,
        BsonSerializationArgs args,
        ProductTitle value)
    {
        context.Writer.WriteString(value.Value);
    }

    public override ProductTitle Deserialize(
        BsonDeserializationContext context,
        BsonDeserializationArgs args)
    {
        return ProductTitle.Create(context.Reader.ReadString()).Value;
    }
}