using Microsoft.Extensions.Options;
using Mongo2Go;
using MongoDB.Driver;
using ShoppingCart.Domain.Models;
using ShoppingCart.Infrastructure.DataAccess;
using Xunit;

namespace ShoppingCart.Infrastructure.Tests
{
    public class MongoShoppingCartRepositoryTests : IDisposable
    {
        private readonly MongoDbRunner _runner;
        private readonly MongoShoppingCartRepository _cartRepository;
        private readonly IMongoCollection<Cart> _cartCollection;

        public MongoShoppingCartRepositoryTests()
        {
            string databaseName = $"testdb_{new Random().Next()}";
            string collectionName = $"testcoll_{new Random().Next()}";

            _runner = MongoDbRunner.Start();

            var settings = new MongoSettings()
            {
                ConnectionString = _runner.ConnectionString,
                Database = databaseName,
                ShoppingCartsCollection = collectionName
            };

            _cartRepository = new MongoShoppingCartRepository(Options.Create(settings));

            _cartCollection = new MongoClient(_runner.ConnectionString)
                .GetDatabase(databaseName)
                .GetCollection<Cart>(collectionName);
        }


        [Fact]
        public async Task FindByCustomer_CartExists_ReturnsCart()
        {
            //Arrange
            var cartsInDatabase = await PrepareDatabase();
            Guid id = cartsInDatabase.Last().Id;
            //Act
            var cart = await _cartRepository.FindByCustomer(id);
            //Assert
            Assert.NotNull(cart);
            Assert.Equal(id, cart.Id);
        }

        [Fact]
        public async Task FindByCustomer_CartDoesNotExist_ReturnsNull()
        {
            //Arrange
            await PrepareDatabase();
            Guid id = Guid.NewGuid();
            //Act
            var cart = await _cartRepository.FindByCustomer(id);
            //Assert
            Assert.Null(cart);
        }

        [Fact]
        public async Task Delete_CartDoesNotExist_ReturnsFalse()
        {
            //Arrange
            var cartsInDatabase = await PrepareDatabase();
            Guid id = Guid.NewGuid();
            //Act
            var deleteResult = await _cartRepository.Delete(id);
            //Assert
            Assert.False(deleteResult);
            long cartCount = await _cartCollection.CountDocumentsAsync(_ => true);
            Assert.Equal(cartsInDatabase.Count, cartCount);
        }

        [Fact]
        public async Task Delete_CartExists_DeletesCartAndReturnsTrue()
        {
            //Arrange
            var cartsInDatabase = await PrepareDatabase();
            Guid id = cartsInDatabase.Last().Id;
            //Act
            var deleteResult = await _cartRepository.Delete(id);
            //Assert
            Assert.True(deleteResult);
            Assert.False(await _cartCollection.Find(cart => cart.Id == id).AnyAsync());
        }


        public async Task<List<Cart>> PrepareDatabase()
        {
            var testCarts = new List<Cart>
            {
                Cart.Create(Guid.Parse("008927df-3b96-4707-ad84-4e689d634987")).Value,
                Cart.Create(Guid.Parse("35454152-4895-42d7-b887-f274deff210d")).Value,
                Cart.Create(Guid.Parse("a7e1c434-8fae-44a9-a6ce-251c29119fc2")).Value
            };

            await _cartCollection.InsertManyAsync(testCarts);
            return testCarts;  
        }

        public void Dispose()
        {
            _runner.Dispose();
        }
    }
}