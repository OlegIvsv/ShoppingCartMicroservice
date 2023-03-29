using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.ValueObjects;
using ShoppingCart.Infrastructure.DataAccess.MongoDb.BsonMapping;
using ShoppingCart.Infrastructure.DataAccess.MongoDb.Serializers;
using ShoppingCart.Interfaces.Interfaces;

namespace ShoppingCart.Infrastructure.DataAccess.MongoDb;

public class MongoShoppingCartRepository : IShoppingCartRepository
{
    private readonly IMongoCollection<Cart> _cartsCollection;

    public MongoShoppingCartRepository(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var shopDb = client.GetDatabase(settings.Value.Database);
        _cartsCollection = shopDb.GetCollection<Cart>(settings.Value.ShoppingCartsCollection);
    }

    static MongoShoppingCartRepository()
    {
        BsonSerializer.RegisterSerializer<Money>(new MoneySerializer());
        BsonSerializer.RegisterSerializer<Quantity>(new QuantitySerializer());
        BsonSerializer.RegisterSerializer<Discount>(new DiscountSerializer());
        BsonSerializer.RegisterSerializer<ProductTitle>(new ProductTitleSerializer());
        BsonSerializer.RegisterSerializer<ImageUrl>(new ImageUrlSerializer());

        new CartItemMapping().RegisterMap();
        new CartMapping().RegisterMap();
    }


    public async Task Save(Cart cart)
    {
        var updateDefinition = Builders<Cart>.Update
            .Set(c => c.Items, cart.Items)
            .Set(c => c.IsAnonymous, cart.IsAnonymous)
            .Set(c => c.LastModifiedDate, DateTime.Now);
        
        var updateOptions = new UpdateOptions() { IsUpsert = true };
        
        await _cartsCollection.UpdateOneAsync(
            c => c.Id == cart.Id,
            updateDefinition,
            updateOptions);
    }

    public async Task<Cart?> FindByCustomer(Guid customerId)
    {
        var cart = await _cartsCollection
            .Find(cart => cart.Id == customerId)
            .FirstOrDefaultAsync();
        return cart;
    }
    
    public async Task<bool> Delete(Guid customerId)
    {
        var deleteResult = await _cartsCollection.DeleteOneAsync(cart => cart.Id == customerId);
        return deleteResult.DeletedCount > 0;
    }

    public async Task<long> DeleteAbandoned(DateTime withoutUpdatesSince)
    {
        var deleteResult = await _cartsCollection
            .DeleteManyAsync(cart => cart.LastModifiedDate < withoutUpdatesSince && cart.IsAnonymous);
        return deleteResult.DeletedCount;
    }
}