using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Feeds.Activities.Extensions;
using Songhay.Feeds.Models;
using Songhay.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
            this.Configuration = configuration;
        }

        public string DisplayHelp(ProgramArgs args) => $@"Downloads the configured Syndication feeds to the configured path and converts them to static JSON.
Use command-line argument {ProgramArgs.BasePath} to prepend a base path to a configured relative path (e.g. FeedsDirectory=""./my-path"").";

        public void Start(ProgramArgs args)
        {
            var meta = this.Configuration.ToFeedsMetadata();
            this.DownloadFeeds(args, meta);
            this.ConvertFeedsToJson(args, meta);
        }

        internal IConfigurationRoot Configuration { get; private set; }

        internal void ConvertFeedsToJson(ProgramArgs args, FeedsMetadata meta)
        {
            var rootDirectory = meta.ToRootDirectory(args);

            meta.Feeds.ForEachInEnumerable(feed =>
            {
                traceSource.TraceVerbose($"feed: {feed.Key}");

                var xml = File.ReadAllText(Path.Combine(rootDirectory, $"{feed.Key}.xml"));
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                var json = JsonConvert.SerializeXmlNode(xmlDoc.DocumentElement, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(Path.Combine(rootDirectory, $"{feed.Key}.json"), json);
            });
        }

        internal void DownloadFeeds(ProgramArgs args, FeedsMetadata meta)
        {
            if (string.IsNullOrEmpty(meta.FeedsDirectory)) throw new NullReferenceException("The expected feeds directory is not configured.");

            var rootDirectory = meta.ToRootDirectory(args);

            var tasks = meta.Feeds.Select(feed =>
            {
                return Task.Run(async () =>
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
            }).ToArray();

            Task.WaitAll(tasks);
        }
    }
}
