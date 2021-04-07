using Microsoft.Extensions.Configuration;
using Songhay.Feeds.Models;
using System;

namespace Songhay.Feeds.Extensions
{
    /// <summary>
    /// Extensions of <see cref="IConfigurationRoot"/>
    /// </summary>
    public static class IConfigurationRootExtensions
    {
        /// <summary>
        /// Convert <see cref="IConfigurationRoot"/> to <see cref="FeedsMetadata"/>.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException">The expected configuration is not here.</exception>
        public static FeedsMetadata ToFeedsMetadata(this IConfigurationRoot configuration)
        {
            if (configuration == null) throw new NullReferenceException("The expected configuration is not here.");

            var meta = new FeedsMetadata();
            configuration.Bind(nameof(FeedsMetadata), meta);

            return meta;
        }
    }
}
