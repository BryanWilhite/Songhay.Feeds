using Quartz;
using Songhay.Extensions;
using Songhay.Models;

namespace Songhay.Feeds.Api.Extensions;

/// <summary>
/// Extensions of <see cref="ITriggerConfigurator"/>
/// </summary>
// ReSharper disable once InconsistentNaming
public static class ITriggerConfiguratorExtensions
{
    public static ITriggerConfigurator WithConfiguredSchedule(this ITriggerConfigurator configurator, RestApiMetadata restApiMetadata)
    {
        const string claimSetKey = "quartz-schedule-mode";
        const string claimSetChronExpressionKey = "quartz-cron-expression";
        const string testing = "testing";

        string? actual = restApiMetadata.ClaimsSet.GetValueWithKey(claimSetKey);
        bool isTesting = testing.Equals(actual);

        if (isTesting)
        {
            configurator.WithSimpleSchedule(
                builder => builder
                    .WithInterval(TimeSpan.FromSeconds(30))
                    .WithRepeatCount(1)
                    .WithMisfireInstruction(SimpleTriggerMisfireInstruction.IgnoreMisfires));

            return configurator;
        }

        string? cronExpression = restApiMetadata
            .ClaimsSet.GetValueWithKey(claimSetChronExpressionKey);

        if (string.IsNullOrWhiteSpace(cronExpression)) return configurator;

        configurator.WithCronSchedule(cronExpression,
            builder => builder
                .WithMisfireInstruction(CronTriggerMisfireInstruction.IgnoreMisfires));

        return configurator;
    }
}
