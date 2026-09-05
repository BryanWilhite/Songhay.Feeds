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
    public static (string? setKey, string? bucketMetaKey, string? bucketKey) ToS3BucketTuplesFromClaimSet(this RestApiMetadata restApiMetadata)
    {
        string? setKey = restApiMetadata
            .ClaimsSet.TryGetValueWithKey("s3-set-key");
        string? bucketMetaKey = restApiMetadata
            .ClaimsSet.TryGetValueWithKey("s3-bucket-meta-key");
        string? bucketKey = restApiMetadata
            .ClaimsSet.TryGetValueWithKey("s3-bucket-key");

        return (setKey, bucketMetaKey, bucketKey);
    }
}
