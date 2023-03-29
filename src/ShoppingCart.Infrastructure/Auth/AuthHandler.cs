using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ShoppingCart.Interfaces.Interfaces;

namespace ShoppingCart.Infrastructure.Auth;

public class AuthHandler : AuthorizationHandler<IsRegisteredRequirement>
{
    private readonly IShoppingCartRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger _logger;

    public AuthHandler(
        IShoppingCartRepository repository, 
        IHttpContextAccessor httpContextAccessor,
        ILoggerFactory loggerFactory)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
        _logger = loggerFactory.CreateLogger<AuthHandler>();
    }
    
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        IsRegisteredRequirement requirement)
    {
        try
        {
            if (context.User.Identity!.IsAuthenticated)
                HandleRegistered(context, requirement);
            else
                await HandleAnonymous(context, requirement);
        }
        catch
        {
            if(_logger.IsEnabled(LogLevel.Error))
                _logger.LogError("An error has occured trying to authorize user");
            context.Fail();
        }
    }

    private async Task HandleAnonymous(AuthorizationHandlerContext context, IsRegisteredRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        string? cartIdStr = httpContext.Request.RouteValues["customerId"]?.ToString();
        bool validCartId = Guid.TryParse(cartIdStr, out Guid cartId);

        if (!validCartId)
        {
            context.Fail();
            return;
        }

        var cartInDb = await _repository.FindByCustomer(cartId);
        
        if (cartInDb?.IsAnonymous ?? true)
            context.Succeed(requirement);
        else
            context.Fail();
    }

    private void HandleRegistered(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        string? cartIdStr = httpContext.Request.RouteValues["customerId"]?.ToString();
        string? userIdStr = context.User.FindFirst(AuthSettings.IdClaimName)?.Value;
        bool validCartId = Guid.TryParse(cartIdStr, out Guid cartId);
        bool validUserId = Guid.TryParse(userIdStr, out Guid userId);
        
        if (!validCartId || !validUserId || cartId != userId)
            context.Fail();
        else
            context.Succeed(requirement);
    }
}