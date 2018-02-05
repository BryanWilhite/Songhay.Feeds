using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Feeds.Activities;
using Songhay.Feeds.Tests.Extensions;
using Songhay.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Songhay.Feeds.Tests.Activities
{
    [TestClass]
    public class DownloadFeedsActivityTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ShouldDisplayHelp()
        {
            var args = new[] { nameof(DownloadFeedsActivity) };
            var activity = this.TestContext.ShouldGetActivityWithConfiguration(this.GetType(), args) as DownloadFeedsActivity;
            Assert.IsNotNull(activity, "The expected activity is not here.");
            this.TestContext.WriteLine(activity.DisplayHelp(new ProgramArgs(args)));
        }

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

        [TestMethod]
        public void ShouldGetRootDirectory()
        {
            var projectDirectoryInfo = this.TestContext.ShouldGetTestProjectDirectoryInfo(this.GetType());

            var args = new[] { nameof(DownloadFeedsActivity), ProgramArgs.BasePath, projectDirectoryInfo.FullName };
            var activity = this.TestContext.ShouldGetActivityWithConfiguration(this.GetType(), args) as DownloadFeedsActivity;
            Assert.IsNotNull(activity, "The expected activity is not here.");

            var meta = activity.GetFeedsMetadata();
            Assert.IsNotNull(meta, "The expected metadata instance is not here.");
            this.TestContext.WriteLine(meta.ToString());

            var root = activity.GetRootDirectory(new ProgramArgs(args), meta);
            this.TestContext.WriteLine(root);
        }

        [TestMethod]
        public void ShouldConvertFeedsToJson()
        {
            var projectDirectoryInfo = this.TestContext.ShouldGetTestProjectDirectoryInfo(this.GetType());

            var args = new[] { nameof(DownloadFeedsActivity), ProgramArgs.BasePath, projectDirectoryInfo.FullName };
            var activity = this.TestContext.ShouldGetActivityWithConfiguration(this.GetType(), args) as DownloadFeedsActivity;
            Assert.IsNotNull(activity, "The expected activity is not here.");

            var meta = activity.GetFeedsMetadata();
            Assert.IsNotNull(meta, "The expected metadata instance is not here.");
            this.TestContext.WriteLine(meta.ToString());

            activity.ConvertFeedsToJson(new ProgramArgs(args), meta);
        }

        [TestMethod]
        public void ShouldDownloadFeeds()
        {
            var projectDirectoryInfo = this.TestContext.ShouldGetTestProjectDirectoryInfo(this.GetType());

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
