namespace ShoppingCart.Infrastructure.BackgroundJobs.CartsCleanUp;

public class CartsCleanUpSettings
{
    public const string SectionName = "Jobs:CartCleanUpJob";
    public TimeSpan CleanUpFrequency { get; set; }
    public TimeSpan AbandonmentPeriod { get; set; }
}