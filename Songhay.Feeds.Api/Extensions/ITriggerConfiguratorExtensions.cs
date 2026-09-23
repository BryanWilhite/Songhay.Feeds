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
        const string claimSetTimeOnlyExpressionKey = "quartz-time-only-expression";
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

        string? timeOnlyExpression = restApiMetadata
            .ClaimsSet.GetValueWithKey(claimSetTimeOnlyExpressionKey);

        if (string.IsNullOrWhiteSpace(timeOnlyExpression))
        {
            return configurator;
        }

        configurator.WithDailyTimeIntervalSchedule(builder =>
            builder
                .OnMondayThroughFriday()
                .StartingDailyAt(TimeOnly.Parse(timeOnlyExpression))
                .WithInterval(24, IntervalUnit.Hour)
                .WithMisfireInstruction(DailyTimeIntervalTriggerMisfireInstruction.DoNothing));

        return configurator;
    }
}
