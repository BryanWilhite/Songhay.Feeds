using Songhay.Extensions;
using Songhay.Models;

namespace Songhay.Feeds.Extensions;

/// <summary>
/// Extensions of <see cref="RestApiMetadata"/>
/// for Studio feeds.
/// </summary>
/// <remarks>
/// Ideally, all magic-string activity
/// on all instances of <see cref="RestApiMetadata"/>
/// </remarks>
public static class RestApiMetadataExtensions
{
    /// <summary>
    /// Transforms the <see cref="RestApiMetadata.ClaimsSet"/>
    /// into a tuple for <see cref="FeedDownloadActivity"/>
    /// </summary>
    /// <param name="restApiMetadata">the <see cref="RestApiMetadata"/></param>
    /// <param name="feedKey">a dictionary key from <see cref="ApiUriSet"/> used to derive the S3 bucket key</param>
    public static (string? setKey, string? bucketMetaKey, string? bucketKey) ToS3BucketTuplesFromClaimSet(this RestApiMetadata restApiMetadata, string? feedKey)
    {
        const string feedKeyPrefix = "feed-";

        string? setKey = restApiMetadata
            .ClaimsSet.TryGetValueWithKey("s3-set-key");
        string? bucketMetaKey = restApiMetadata
            .ClaimsSet.TryGetValueWithKey("s3-bucket-meta-key");
        string? bucketKey = restApiMetadata
            .ClaimsSet.TryGetValueWithKey("s3-bucket-key");

        return (setKey, bucketMetaKey, $"{bucketKey}/{feedKey?.Replace(feedKeyPrefix, string.Empty)}");
    }
}
