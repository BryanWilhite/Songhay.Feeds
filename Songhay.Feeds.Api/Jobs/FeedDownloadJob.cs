using Quartz;

using Songhay.Abstractions;
using Songhay.Feeds.Activities;

namespace Songhay.Feeds.Api.Jobs;

[DisallowConcurrentExecution]
public sealed class FeedDownloadJob(
    [FromKeyedServices(nameof(FeedDownloadActivity))] IActivityTask activity
) : IJob
{
    public async ValueTask Execute(IJobExecutionContext context, CancellationToken cancellationToken = default)
    {
        await activity.StartAsync();

        await Task.CompletedTask;
    }
}
