using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Feeds.Models;
using Songhay.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Xml;

namespace Songhay.Feeds.Activities
{
    public class DownloadFeedsActivity : IActivity, IActivityConfigurationSupport
    {
        static DownloadFeedsActivity() => traceSource = TraceSources
            .Instance
            .GetTraceSourceFromConfiguredName()
            .WithAllSourceLevels()
            .EnsureTraceSource();

        static readonly TraceSource traceSource;

        public void AddConfiguration(IConfigurationRoot configuration)
        {
            this._configuration = configuration;
        }

        public string DisplayHelp(ProgramArgs args) => "Downloads the configured Syndication feeds and converts them to static JSON.";

        public void Start(ProgramArgs args)
        {
            var meta = this.GetFeedsMetadata();
            this.DownloadFeeds(args, meta);
            this.ConvertFeedsToJson(args, meta);
        }

        internal void ConvertFeedsToJson(ProgramArgs args, FeedsMetadata meta)
        {
            var rootDirectory = GetRootDirectory(args, meta);

            meta.Feeds.ForEachInEnumerable(feed =>
            {
                traceSource.TraceVerbose($"feed: {feed.Key}");

                var xml = File.ReadAllText(Path.Combine(rootDirectory, $"{feed.Key}.xml"));
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                var json = JsonConvert.SerializeXmlNode(xmlDoc.FirstChild, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(Path.Combine(rootDirectory, $"{feed.Key}.json"), json);
            });
        }

        internal void DownloadFeeds(ProgramArgs args, FeedsMetadata meta)
        {
            if (string.IsNullOrEmpty(meta.FeedsDirectory)) throw new NullReferenceException("The expected feeds directory is not configured.");

            var rootDirectory = GetRootDirectory(args, meta);

            meta.Feeds.ForEachInEnumerable(async feed =>
            {
                var uri = new Uri(feed.Value, UriKind.Absolute);
                traceSource.TraceVerbose($"uri: {uri.OriginalString}");
                var client = new HttpClient();
                var request = new HttpRequestMessage() { RequestUri = uri, Method = HttpMethod.Get };

                var response = await client.SendAsync(request);
                if (response == null) throw new NullReferenceException("The expected response is not here.");
                if (response.StatusCode != HttpStatusCode.OK) throw new HttpRequestException("The expected response status code is not here.");

                var xml = await response.Content.ReadAsStringAsync();
                File.WriteAllText(Path.Combine(rootDirectory, $"{feed.Key}.xml"), xml);
            });
        }

        internal FeedsMetadata GetFeedsMetadata()
        {
            if (this._configuration == null) throw new NullReferenceException("The expected configuration is not here.");

            var meta = new FeedsMetadata();
            this._configuration.Bind(nameof(FeedsMetadata), meta);

            return meta;
        }

        internal string GetRootDirectory(ProgramArgs args, FeedsMetadata meta)
        {
            var basePath = args.HasArg(ProgramArgs.BasePath, requiresValue: false) ? args.GetBasePathValue() : Directory.GetCurrentDirectory();

            var rootDirectory = meta.FeedsDirectory.StartsWith("./") ?
                Path.GetFullPath(Path.Combine(basePath, meta.FeedsDirectory))
                :
                meta.FeedsDirectory;

            if (!Directory.Exists(rootDirectory)) throw new DirectoryNotFoundException("The expected root directory is not here.");

            return rootDirectory;
        }

        IConfigurationRoot _configuration;
    }
}
