using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using Songhay.Feeds.Activities;
using Songhay.Feeds.Activities.Extensions;
using Songhay.Feeds.Shell;
using Songhay.Feeds.Tests.Extensions;
using Songhay.Models;
using System;
using System.Diagnostics;

namespace Songhay.Feeds.Tests.Activities
{
    [TestClass]
    public class DownloadFeedsActivityTest
    {
        public TestContext TestContext { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void ShouldDisplayHelp()
        {
            var args = new[] { nameof(DownloadFeedsActivity) };
            var activity = this.TestContext.ShouldGetActivityWithConfiguration(this.GetType(), args) as DownloadFeedsActivity;
            Assert.IsNotNull(activity, "The expected activity is not here.");
            this.TestContext.WriteLine(activity.DisplayHelp(new ProgramArgs(args)));
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ShouldGetFeedsMetadata()
        {
            var args = new[] { nameof(DownloadFeedsActivity) };
            var activity = this.TestContext.ShouldGetActivityWithConfiguration(this.GetType(), args) as DownloadFeedsActivity;
            Assert.IsNotNull(activity, "The expected activity is not here.");

            var meta = activity.Configuration.ToFeedsMetadata();
            Assert.IsNotNull(meta, "The expected metadata instance is not here.");
            this.TestContext.WriteLine(meta.ToString());
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ShouldGetRootDirectory()
        {
            var projectDirectoryInfo = this.TestContext.ShouldGetProjectDirectoryInfo(this.GetType());

            var args = new[] { nameof(DownloadFeedsActivity), ProgramArgs.BasePath, projectDirectoryInfo.FullName };
            var activity = this.TestContext.ShouldGetActivityWithConfiguration(this.GetType(), args) as DownloadFeedsActivity;
            Assert.IsNotNull(activity, "The expected activity is not here.");

            var meta = activity.Configuration.ToFeedsMetadata();
            Assert.IsNotNull(meta, "The expected metadata instance is not here.");
            this.TestContext.WriteLine(meta.ToString());

            var root = meta.ToRootDirectory(new ProgramArgs(args));
            this.TestContext.WriteLine(root);
        }

        [Ignore("The build server should ignore this test because it depends on ShouldDownloadFeeds().")]
        [TestCategory("Integration")]
        [TestMethod]
        public void ShouldConvertFeedsToJson()
        {
            var projectDirectoryInfo = this.TestContext.ShouldGetProjectDirectoryInfo(this.GetType());

            var args = new[] { nameof(DownloadFeedsActivity), ProgramArgs.BasePath, projectDirectoryInfo.FullName };
            var activity = this.TestContext.ShouldGetActivityWithConfiguration(this.GetType(), args) as DownloadFeedsActivity;
            Assert.IsNotNull(activity, "The expected activity is not here.");

            var meta = activity.Configuration.ToFeedsMetadata();
            Assert.IsNotNull(meta, "The expected metadata instance is not here.");
            this.TestContext.WriteLine(meta.ToString());

            var listener = new TextWriterTraceListener(Console.Out);
            try
            {
                Program.InitializeTraceSource(listener);
                activity.ConvertFeedsToJson(new ProgramArgs(args), meta);
            }
            finally
            {
                listener.Flush();
            }
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ShouldDownloadFeeds()
        {
            var projectDirectoryInfo = this.TestContext.ShouldGetProjectDirectoryInfo(this.GetType());

            var args = new[] { nameof(DownloadFeedsActivity), ProgramArgs.BasePath, projectDirectoryInfo.FullName };
            var activity = this.TestContext.ShouldGetActivityWithConfiguration(this.GetType(), args) as DownloadFeedsActivity;
            Assert.IsNotNull(activity, "The expected activity is not here.");

            var meta = activity.Configuration.ToFeedsMetadata();
            Assert.IsNotNull(meta, "The expected metadata instance is not here.");
            this.TestContext.WriteLine(meta.ToString());

            var listener = new TextWriterTraceListener(Console.Out);
            try
            {
                Program.InitializeTraceSource(listener);
                activity.DownloadFeeds(new ProgramArgs(args), meta);
            }
            finally
            {
                listener.Flush();
            }
        }
    }
}
