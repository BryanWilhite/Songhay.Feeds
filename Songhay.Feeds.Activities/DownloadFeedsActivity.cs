using Microsoft.Extensions.Configuration;
using Songhay.Diagnostics;
using Songhay.Extensions;
using Songhay.Models;
using System.Diagnostics;
using System.Linq;

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

        public string DisplayHelp(ProgramArgs args)
        {
            if (!args.HelpSet.Any()) this.SetupHelp(args);

            return args.ToHelpDisplayText();
        }

        public void Start(ProgramArgs args)
        {
        }

        void SetupHelp(ProgramArgs args)
        {
            var indentation = string.Join(string.Empty, Enumerable.Repeat(" ", 4).ToArray());
            args.HelpSet.Add(argDownloadFeeds, $"{argDownloadFeeds}{indentation}Downloads the configured Syndication feeds and converts them to static JSON.");
        }

        IConfigurationRoot _configuration;

        const string argDownloadFeeds = "--download-feeds";
    }
}
