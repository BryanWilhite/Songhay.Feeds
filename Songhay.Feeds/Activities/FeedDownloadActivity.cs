using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Songhay.Abstractions;
using Songhay.Feeds.Models;
using Songhay.Models;
using Songhay.Web.Models;

namespace Songhay.Feeds.Activities;

public class FeedDownloadActivity(
        IActivityKeyedTaskGroup amazonS3ActivityGroup,
        [FromKeyedServices(ApiKeyConstants.DepKeyForRestApiMetadata)] RestApiMetadata restApiMetadataForThisApp,
        [FromKeyedServices(FeedsConstants.DepKeyForWasabi)] RestApiMetadata restApiMetadataForWasabi,
        IHttpClientFactory httpClientFactory,
        ILogger<FeedDownloadActivity> logger
    ) : IActivityTask
{
    public Task StartAsync()
    {
        throw new NotImplementedException();
    }
}
