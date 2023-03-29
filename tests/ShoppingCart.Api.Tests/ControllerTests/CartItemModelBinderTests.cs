using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShoppingCart.Api.Contracts.ContractBinders;
using ShoppingCart.Domain.Entities;
using System.Text.Json;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllerTests;

public class CartItemBinderTests
{
    [Fact]
    public async Task BindModelAsync_WhenRequestIsNotJson_ReturnsUnsupportedMediaTypeError()
    {
        // Arrange
        var bindingContext = GetTestBindingContext();
        bindingContext.HttpContext.Request.ContentType = "application/xml";
        var binder = new CartItemBinder();
        // Act
        await binder.BindModelAsync(bindingContext);
        // Assert
        Assert.False(bindingContext.Result.IsModelSet);
        Assert.Null(bindingContext.Result.Model);
        Assert.Single(bindingContext.ModelState);
    }

    [Fact]
    public async Task BindModelAsync_WhenRequestIsInvalidJson_ReturnsObjectFormatError()
    {
        // Arrange
        var bindingContext = GetTestBindingContext();
        bindingContext.HttpContext.Request.ContentType = "application/json";
        bindingContext.HttpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("{"));
        var binder = new CartItemBinder();
        // Act
        await binder.BindModelAsync(bindingContext);
        // Assert
        Assert.False(bindingContext.Result.IsModelSet);
        Assert.Null(bindingContext.Result.Model);
        Assert.Single(bindingContext.ModelState);
        Assert.Equal("ObjectFormatError", bindingContext.ModelState.First().Key);
    }

    [Fact]
    public async Task BindModelAsync_WhenRequestIsValid_ReturnsModel()
    {
        // Arrange
        var itemRequest = new {
            productId = Guid.NewGuid(),
            unitPrice = 12.34m,
            productTitle = "Product Title",
            itemQuantity = 2,
            discount = 0.1,
            imageUrl = "http://example.com/image.jpg"
        };

        var bindingContext = GetTestBindingContext();
        bindingContext.HttpContext.Request.ContentType = "application/json";
        bindingContext.HttpContext.Request.Body = new MemoryStream(
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(itemRequest)));
        var binder = new CartItemBinder();
        
        // Act
        await binder.BindModelAsync(bindingContext);
        
        // Assert
        Assert.True(bindingContext.Result.IsModelSet);
        Assert.IsType<CartItem>(bindingContext.Result.Model);
        var cartItem = (CartItem)bindingContext.Result.Model;
        Assert.Equal(itemRequest.productId, cartItem.ProductId);
        Assert.Equal(itemRequest.productTitle, cartItem.ProductTitle.Value);
        Assert.Equal(itemRequest.itemQuantity, cartItem.ItemQuantity.Value);
        Assert.Equal(itemRequest.unitPrice, cartItem.UnitPrice.Value);
        Assert.Equal(itemRequest.discount, cartItem.Discount.Value);
        Assert.Equal(itemRequest.imageUrl, cartItem.Image.Value);
    }
    
    [Fact]
    public async Task BindModelAsync_WhenRequestIsNotValid_FillsModelStateWithErrors()
    {
        // Arrange
        var itemRequest = new
        {
            productId = Guid.NewGuid(),
            unitPrice = -12.34m,
            productTitle = "",
            itemQuantity = -1,
            discount = 1.01,
            imageUrl = "text"
        };

        var bindingContext = GetTestBindingContext();
        bindingContext.HttpContext.Request.ContentType = "application/json";
        bindingContext.HttpContext.Request.Body = new MemoryStream(
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(itemRequest)));
        var binder = new CartItemBinder();

        // Act
        await binder.BindModelAsync(bindingContext);

        // Assert
        Assert.False(bindingContext.Result.IsModelSet);
        Assert.True(
            bindingContext.ModelState.ErrorCount == 5, 
            $"Not all errors were set ({bindingContext.ModelState.Count} set)!");
    }

    private ModelBindingContext GetTestBindingContext()
    {
        var httpContext = new DefaultHttpContext();
        var context =  new DefaultModelBindingContext
        {
            ActionContext = new ActionContext { HttpContext = httpContext },
            ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(CartItem)),
            ModelState = new ModelStateDictionary()
        };
        return context;
    }
}
