using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Feeds.Activities;
using Songhay.Feeds.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Songhay.Feeds.Tests.Activities
{
    [TestClass]
    public class DownloadFeedsActivityTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ShouldGetFeedsMetadata()
        {
            var shellProjectDirectoryInfo = this.TestContext.ShouldGetShellProjectDirectoryInfo(this.GetType());
            var configuration = Shell.Program.LoadConfiguration(shellProjectDirectoryInfo.FullName);
            Assert.IsNotNull(configuration, "The expected configuration is not here.");

            var args = new[] { nameof(DownloadFeedsActivity) };
            var getter = Shell.Program.GetActivitiesGetter(args);
            Assert.IsNotNull(getter, "The expected activities getter is not here.");

            var activity = getter.GetActivity() as DownloadFeedsActivity;
            Assert.IsNotNull(activity, "The expected activity is not here.");

            activity.AddConfiguration(configuration);
            var meta = activity.GetFeedsMetadata();
            Assert.IsNotNull(meta, "The expected metadata instance is not here.");
            this.TestContext.WriteLine(meta.ToString());
        }
    }
}
