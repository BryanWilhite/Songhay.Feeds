using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Songhay.Abstractions;
using Songhay.Extensions;
using Songhay.Models;
using Songhay.S3.Activities;
using Songhay.Web.Models;

namespace Songhay.Feeds.Activities;

public class FeedDownloadActivity(
        IActivityKeyedTaskGroup amazonS3ActivityGroup,
        [FromKeyedServices(ApiKeyConstants.DepKeyForRestApiMetadata)] RestApiMetadata restApiMetadataForThisApp,
        IHttpClientFactory httpClientFactory,
        ILogger<FeedDownloadActivity> logger
    ) : IActivityTask
{
    public async Task StartAsync()
    {
        logger.LogInformation("Loading feeds information...");

        KeyValuePair<string, string>[] feedsSet =
            [.. restApiMetadataForThisApp.ClaimsSet
                .Where(kv => kv.Key.StartsWith("feed-"))];

        HttpClient httpClientForDownload =
            httpClientFactory.CreateClient("feed-download-client");
        HttpClient httpClientForUpload =
            httpClientFactory.CreateClient("feed-upload-client");

        logger.LogInformation("Fetching feeds...");

        foreach (KeyValuePair<string, string> feed in feedsSet)
        {
            logger.LogDebug("Fetching {Name} ({Uri})...", feed.Key, feed.Value);

            Uri uri = new(feed.Value, UriKind.Absolute);
            HttpRequestMessage request = new(HttpMethod.Get, uri);
            HttpResponseMessage response = await httpClientForDownload.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("Response is {Code} ({Phrase})! Continuing...",
                    response.StatusCode,
                    response.ReasonPhrase);

                continue;
            }

            string content = await response.Content.ReadAsStringAsync();

            logger.LogDebug("Saving {Name} ({Uri})...", feed.Key, feed.Value);

            string? setKey = restApiMetadataForThisApp
                .ClaimsSet.TryGetValueWithKey("s3-set-key");
            string? bucketMetaKey = restApiMetadataForThisApp
                .ClaimsSet.TryGetValueWithKey("s3-bucket-meta-key");
            string? bucketKey = restApiMetadataForThisApp
                .ClaimsSet.TryGetValueWithKey("s3-bucket-key");
            const string contentMimeType = MimeTypes.ApplicationXml;

            await amazonS3ActivityGroup
                .InvokeActivityAsync(nameof(AmazonS3UploadStringActivity),
                    setKey, bucketMetaKey, bucketKey, content, contentMimeType
                );
        }

        logger.LogInformation("Feeds processed. Exiting...");
    }
}
