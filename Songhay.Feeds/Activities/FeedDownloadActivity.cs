using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Songhay.Abstractions;
using Songhay.Feeds.Extensions;
using Songhay.Models;
using Songhay.S3.Activities;
using Songhay.Web.Models;

namespace Songhay.Feeds.Activities;

public class FeedDownloadActivity(
        IActivityKeyedTaskGroup amazonS3ActivityGroup,
        ApiUriSet feedsSet,
        [FromKeyedServices(ApiKeyConstants.DepKeyForRestApiMetadata)] RestApiMetadata restApiMetadata,
        IHttpClientFactory httpClientFactory,
        ILogger<FeedDownloadActivity> logger
    ) : IActivityTask
{
    public async Task StartAsync()
    {
        logger.LogInformation("Loading feeds information...");

        HttpClient httpClientForDownload =
            httpClientFactory.CreateClient(nameof(FeedDownloadActivity));

        logger.LogInformation("Fetching feeds...");

        foreach (KeyValuePair<string, Uri> feed in feedsSet)
        {
            logger.LogDebug("Fetching {Name} ({Uri})...", feed.Key, feed.Value);

            HttpRequestMessage request = new(HttpMethod.Get, feed.Value);
            using HttpResponseMessage response = await httpClientForDownload.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Response is {Code} ({Phrase})! Continuing...",
                    response.StatusCode,
                    response.ReasonPhrase);

                continue;
            }

            string content = await response.Content.ReadAsStringAsync();

            logger.LogDebug("Saving {Name} ({Uri})...", feed.Key, feed.Value);

            var (setKey, bucketMetaKey, bucketKey) = restApiMetadata.ToS3BucketTuplesFromClaimSet();
            const string contentMimeType = MimeTypes.ApplicationXml;

            await amazonS3ActivityGroup
                .InvokeActivityAsync(nameof(AmazonS3UploadStringActivity),
                    setKey, bucketMetaKey, bucketKey, content, contentMimeType
                );
        }

        logger.LogInformation("Feeds processed. Exiting...");
    }
}
