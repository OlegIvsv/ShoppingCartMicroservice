using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using ShoppingCart.Infrastructure.BackgroundJobs.CartsCleanUp;

namespace ShoppingCart.Infrastructure.BackgroundJobs;

public static class JobExtensions
{
    public static IServiceCollection AddQuartzCleanUpJob(
        this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        var cleanUpJobSettings = new CartsCleanUpSettings();
        configurationManager.Bind(CartsCleanUpSettings.SectionName, cleanUpJobSettings);
        services.AddSingleton(Options.Create(cleanUpJobSettings));

        if (!cleanUpJobSettings.Enabled)
            return services;

        services.AddQuartz(quartz =>
        {
            quartz.UseInMemoryStore();
            quartz.UseMicrosoftDependencyInjectionJobFactory();

            var cleanerJobKey = CartsCleanUpJob.Key;
            quartz.AddJob<CartsCleanUpJob>(opts =>
                opts.WithIdentity(cleanerJobKey));

            var triggerKey = new TriggerKey("carts-cleanup-trigger");
            quartz.AddTrigger(configure => configure
                .WithIdentity(triggerKey)
                .WithSimpleSchedule(sched => sched
                    .RepeatForever()
                    .WithInterval(cleanUpJobSettings.CleanUpFrequency))
                .ForJob(CartsCleanUpJob.Key)
                .StartNow());
        });

        services.AddQuartzHostedService(config => config.WaitForJobsToComplete = true);

        return services;
    }
}