﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using Songhay.Feeds.Activities;
using Songhay.Feeds.Tests.Extensions;
using Songhay.Models;

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

            var meta = activity.GetFeedsMetadata();
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

            var meta = activity.GetFeedsMetadata();
            Assert.IsNotNull(meta, "The expected metadata instance is not here.");
            this.TestContext.WriteLine(meta.ToString());

            var root = activity.GetRootDirectory(new ProgramArgs(args), meta);
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

            var meta = activity.GetFeedsMetadata();
            Assert.IsNotNull(meta, "The expected metadata instance is not here.");
            this.TestContext.WriteLine(meta.ToString());

            activity.ConvertFeedsToJson(new ProgramArgs(args), meta);
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void ShouldDownloadFeeds()
        {
            var projectDirectoryInfo = this.TestContext.ShouldGetProjectDirectoryInfo(this.GetType());

            var args = new[] { nameof(DownloadFeedsActivity), ProgramArgs.BasePath, projectDirectoryInfo.FullName };
            var activity = this.TestContext.ShouldGetActivityWithConfiguration(this.GetType(), args) as DownloadFeedsActivity;
            Assert.IsNotNull(activity, "The expected activity is not here.");

            var meta = activity.GetFeedsMetadata();
            Assert.IsNotNull(meta, "The expected metadata instance is not here.");
            this.TestContext.WriteLine(meta.ToString());

            activity.DownloadFeeds(new ProgramArgs(args), meta);
        }
    }
}
