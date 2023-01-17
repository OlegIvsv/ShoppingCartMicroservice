using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Api.Contracts.ContractAttributes;

[AttributeUsage(
    AttributeTargets.Parameter |
    AttributeTargets.Field | 
    AttributeTargets.Property)]
public class GuidIdAttribute : ValidationAttribute
{
    public GuidIdAttribute()
        : base("The value is not a valid guid id")
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is not Guid guidValue)
            return false;
        if (guidValue == Guid.Empty)
            return false;
        return true;
    }
}