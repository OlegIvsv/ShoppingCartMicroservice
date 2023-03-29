using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using ShoppingCart.Interfaces.Interfaces;

namespace ShoppingCart.Infrastructure.BackgroundJobs.CartsCleanUp;

public class CartsCleanUpJob : IJob
{
    public static readonly JobKey Key = new("carts-cleanup-job", "carts-cleanup");
    
    private readonly IShoppingCartRepository _repository;
    private readonly ILogger<CartsCleanUpJob> _logger;
    private readonly TimeSpan _abandonmentPeriod;

    public CartsCleanUpJob(
        ILoggerFactory loggerFactory, 
        IShoppingCartRepository repository,
        IOptions<CartsCleanUpSettings> settings)
    {
        _logger = loggerFactory.CreateLogger<CartsCleanUpJob>();
        _repository = repository;
        _abandonmentPeriod = settings.Value.AbandonmentPeriod;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Deleting abandoned anonymous carts");

        var withoutUpdatesSince = DateTime.Now.Subtract(_abandonmentPeriod);
        long deletedCount = await _repository.DeleteAbandoned(withoutUpdatesSince);

        _logger.LogInformation(
            "Carts deleted:{0}. Haven't been updated since: {1}",
            deletedCount,
            withoutUpdatesSince);
    }
}















