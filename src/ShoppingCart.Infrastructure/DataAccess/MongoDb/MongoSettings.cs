namespace ShoppingCart.Infrastructure.DataAccess.MongoDb;

public class MongoSettings
{
    public const string SectionName = "MongoDb";
    public string ConnectionString { get; init; }
    public string Database { get; init; }
    public string ShoppingCartsCollection { get; init; }
}