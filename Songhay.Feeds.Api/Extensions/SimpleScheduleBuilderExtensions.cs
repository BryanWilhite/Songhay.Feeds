using Quartz;
using Songhay.Extensions;
using Songhay.Models;

namespace Songhay.Feeds.Api.Extensions;

/// <summary>
/// Extensions for <see cref="SimpleScheduleBuilder"/>
/// </summary>
public static class SimpleScheduleBuilderExtensions
{
    /// <summary>
    /// Returns <see cref="SimpleScheduleBuilder"/>
    /// with the current scheduling conventions
    /// for the <see cref="IJob"/> of this app.
    /// </summary>
    /// <param name="builder">the <see cref="SimpleScheduleBuilder"/></param>
    /// <param name="restApiMetadata">the <see cref="RestApiMetadata"/> used to configure scheduling</param>
    /// <remarks>
    /// The <see cref="RestApiMetadata"/> is used
    /// to determine whether it should be scheduled for a manual testing environment
    /// or a production environment.
    /// </remarks>
    public static SimpleScheduleBuilder WithConventions(this SimpleScheduleBuilder builder, RestApiMetadata restApiMetadata)
    {
        const string claimSetKey = "quartz-schedule-mode";
        const string testing = "testing";

        string? actual = restApiMetadata.ClaimsSet.TryGetValueWithKey(claimSetKey);
        bool isTesting = testing.Equals(actual);
        if (isTesting)
        {
            builder
                .WithInterval(TimeSpan.FromSeconds(30))
                .WithRepeatCount(1);
        }
        else
        {
            builder
                .WithInterval(TimeSpan.FromHours(24))
                .RepeatForever();
        }

        return builder;
    }
}
