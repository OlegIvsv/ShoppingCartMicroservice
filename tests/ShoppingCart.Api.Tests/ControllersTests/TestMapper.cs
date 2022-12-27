using Mapster;
using MapsterMapper;
using System.Reflection;

namespace ShoppingCart.Api.Tests.ControllersTests
{
    internal static class MapsterForTests
    {
        public static IMapper GetMapper()
        {
            TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            return new Mapper(config);
        }
    }
}
