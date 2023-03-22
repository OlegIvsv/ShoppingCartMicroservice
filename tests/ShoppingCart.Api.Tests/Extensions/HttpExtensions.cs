using System.Net;
using Xunit;

namespace ShoppingCart.Api.Tests.ControllersTests.Extensions;

internal static class HttpResponseMessageExtensions
{
    public static void AssertJsonUtf8(this HttpResponseMessage responseMessage)
    {
        Assert.Equal(
            "application/json; charset=utf-8",
            responseMessage.Content.Headers.ContentType.ToString());
    }

    public static void AssertJsonProblemUtf8(this HttpResponseMessage responseMessage)
    {
        Assert.Equal(
            "application/problem+json; charset=utf-8",
            responseMessage.Content.Headers.ContentType.ToString());
    }
    
    public static void AssertBadRequest(this HttpResponseMessage responseMessage)
    {
        Assert.Equal(HttpStatusCode.BadRequest, responseMessage.StatusCode);
    }

    public static void AssertNotFound(this HttpResponseMessage responseMessage)
    {
        Assert.Equal(HttpStatusCode.NotFound, responseMessage.StatusCode);
    }

    public static void AssertCondlict(this HttpResponseMessage responseMessage)
    {
        Assert.Equal(HttpStatusCode.Conflict, responseMessage.StatusCode);
    }

    public static void AssertUnauthorized(this HttpResponseMessage responseMessage)
    {
        Assert.Equal(HttpStatusCode.Unauthorized, responseMessage.StatusCode);
    }

    public static void AssertCreated(this HttpResponseMessage responseMessage)
    {
        Assert.Equal(HttpStatusCode.Created, responseMessage.StatusCode);
    }

    public static void AssertOK(this HttpResponseMessage responseMessage)
    {
        Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
    }

    public static void AssertStatus(this HttpResponseMessage responseMessage, HttpStatusCode status)
    {
        Assert.Equal(status, responseMessage.StatusCode);
    }
}