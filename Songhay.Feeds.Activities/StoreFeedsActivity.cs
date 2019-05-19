using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Songhay.Cloud.BlobStorage.Extensions;
using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Feeds.Activities.Extensions;
using Songhay.Feeds.Models;
using Songhay.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Songhay.Feeds.Activities
{
    public class StoreFeedsActivity : IActivity, IActivityConfigurationSupport
    {
        static StoreFeedsActivity() => traceSource = TraceSources
            .Instance
            .GetConfiguredTraceSource();

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

            var cloudStorageAccount = this.GetCloudStorageAccount(meta,
                cloudStorageSetName: "SonghayCloudStorage",
                connectionStringName: "general-purpose-v1");

            var feedsMeta = this.Configuration.ToFeedsMetadata();

            this.UploadJson(cloudStorageAccount, feedsMeta, args);
        }

        internal IConfigurationRoot Configuration { get; private set; }

        internal CloudStorageAccount GetCloudStorageAccount(ProgramMetadata meta, string cloudStorageSetName, string connectionStringName)
        {
            //TODO: move this to an extension method in Cloud storage core:
            if (meta.CloudStorageSet == null) throw new NullReferenceException("The expected cloud storage set is not here.");

            var key = cloudStorageSetName;

            var test = meta.CloudStorageSet.TryGetValue(key, out var set);

            if (!test) throw new NullReferenceException($"The expected cloud storage set, {key}, is not here.");
            if (!set.Any()) throw new NullReferenceException($"The expected cloud storage set items for {key} are not here.");
            test = set.TryGetValue(connectionStringName, out var connectionString);
            if (!test) throw new NullReferenceException($"The expected cloud storage set connection, {connectionStringName}, is not here.");

            var cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            return cloudStorageAccount;
        }

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

            var container = cloudStorageAccount.GetContainerReference("studio-dash");

            var tasks = feedsMeta.Feeds.Keys.Select(i =>
            {
                var jsonFile = root.ToCombinedPath($"{i}.json");
                traceSource?.TraceVerbose($"uploading {jsonFile}...");
                return container.UploadBlobAsync(jsonFile, string.Empty);
            }).ToArray();

            Task.WaitAll(tasks);
        }
    }
}
