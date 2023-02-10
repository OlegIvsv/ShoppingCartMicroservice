using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using ShoppingCart.Domain.ValueObjects;

namespace ShoppingCart.Infrastructure.DataAccess.MongoDb.Serializers;

internal class ImageUrlSerializer : SerializerBase<ImageUrl>
{
    public override void Serialize(
        BsonSerializationContext context,
        BsonSerializationArgs args,
        ImageUrl value)
    {
        context.Writer.WriteString(value.Value);
    }

    public override ImageUrl Deserialize(
        BsonDeserializationContext context,
        BsonDeserializationArgs args)
    {
        return ImageUrl.Create(
            BsonSerializer.Deserialize<string>(context.Reader)).Value;
    }
}