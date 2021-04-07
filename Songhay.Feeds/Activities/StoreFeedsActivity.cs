using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Songhay.Cloud.BlobStorage.Extensions;
using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Feeds.Extensions;
using Songhay.Feeds.Models;
using Songhay.Models;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Songhay.Feeds.Activities
{
    public class StoreFeedsActivity : IActivity, IActivityConfigurationSupport
    {
        static StoreFeedsActivity() => traceSource = TraceSources
            .Instance
            .GetConfiguredTraceSource()
            .WithSourceLevels();

        static readonly TraceSource traceSource;

        public void AddConfiguration(IConfigurationRoot configuration)
        {
            this.Configuration = configuration;
        }

        public string DisplayHelp(ProgramArgs args) => $@"Uploads JSON files from {nameof(DownloadFeedsActivity)} to Studio Dashboard cloud storage.";

        public void Start(ProgramArgs args)
        {
            traceSource?.TraceInformation($"starting {nameof(StoreFeedsActivity)}...");

            var meta = this.GetProgramMetadata();

            var cloudStorageAccount = meta.GetCloudStorageAccount(cloudStorageSetName: "SonghayCloudStorage", connectionStringName: "general-purpose-v1");

            var feedsMeta = this.Configuration.ToFeedsMetadata();

            this.UploadJson(cloudStorageAccount, feedsMeta, args);
        }

        internal IConfigurationRoot Configuration { get; private set; }

        internal ProgramMetadata GetProgramMetadata()
        {
            var meta = new ProgramMetadata();
            var key = nameof(ProgramMetadata);
            ConfigurationBinder.Bind(this.Configuration, key, meta);
            return meta;
        }

        internal void UploadJson(CloudStorageAccount cloudStorageAccount, FeedsMetadata feedsMeta, ProgramArgs args)
        {
            var root = feedsMeta.ToRootDirectory(args);
            var rootInfo = new DirectoryInfo(root);

            var container = cloudStorageAccount.GetContainerReference("studio-dash");

            var tasks = feedsMeta.Feeds.Keys.Select(i =>
            {
                var jsonFile = rootInfo.ToCombinedPath($"{i}.json");
                if(!File.Exists(jsonFile))
                {
                    traceSource?.TraceWarning($"The expected file, `{jsonFile}`, is not here.");
                    return Task.FromResult(0);
                }

                traceSource?.TraceVerbose($"uploading {jsonFile}...");
                return container.UploadBlobAsync(jsonFile, string.Empty);
            }).ToArray();

            Task.WaitAll(tasks);
        }
    }
}
