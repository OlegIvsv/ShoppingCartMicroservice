namespace ShoppingCart.Api.Contracts.RouteConstraints;

public class GuidIdConstraint : IRouteConstraint
{
    public bool Match(
        HttpContext httpContext, 
        IRouter route, 
        string routeKey,
        RouteValueDictionary values, 
        RouteDirection routeDirection)
    {
        var value = values[routeKey]?.ToString();
        if(value is null) 
            return false;
        return Guid.TryParse(value, out Guid parsedId) && parsedId != Guid.Empty;
    }
}