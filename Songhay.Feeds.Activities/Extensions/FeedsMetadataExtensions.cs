using Songhay.Extensions;
using Songhay.Feeds.Models;
using Songhay.Models;
using System.IO;

namespace Songhay.Feeds.Activities.Extensions
{
    /// <summary>
    /// Extension of <see cref="FeedsMetadata"/>
    /// </summary>
    public static class FeedsMetadataExtensions
    {
        /// <summary>
        /// Converts <see cref="FeedsMetadata"/> to root directory.
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        /// <exception cref="DirectoryNotFoundException">The expected root directory is not here.</exception>
        public static string ToRootDirectory(this FeedsMetadata meta, ProgramArgs args)
        {
            var basePath = args.HasArg(ProgramArgs.BasePath, requiresValue: false) ? args.GetBasePathValue() : Directory.GetCurrentDirectory();

            var rootDirectory = meta.FeedsDirectory.StartsWith("./") ?
                basePath.ToCombinedPath(meta.FeedsDirectory)
                :
                meta.FeedsDirectory;

            rootDirectory = Path.GetFullPath(rootDirectory);

            if (!Directory.Exists(rootDirectory)) throw new DirectoryNotFoundException("The expected root directory is not here.");

            return rootDirectory;
        }
    }
}
