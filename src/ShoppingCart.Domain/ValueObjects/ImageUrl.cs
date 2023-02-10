using FluentResults;
using ShoppingCart.Domain.Common;
using ShoppingCart.Domain.Errors;

namespace ShoppingCart.Domain.ValueObjects;

public sealed class ImageUrl : ValueObject<ImageUrl>
{
    public string Value { get; }

    private ImageUrl(string url)
    {
        Value = url;
    }

    public static Result<ImageUrl> Create(string url)
    {
        bool isValidFormat = Uri.TryCreate(url, UriKind.Absolute, out Uri? uriInfo);
        bool isValidSchema = uriInfo.Scheme == Uri.UriSchemeHttp
                             || uriInfo.Scheme == Uri.UriSchemeHttps;
        if(isValidFormat && isValidSchema)
            return new ImageUrl(url);
        
        return Result.Fail(new InvalidImageUrlError(url));
    }

    protected override IEnumerable<object> AtomicValuesList()
    {
        yield return Value;
    }
}