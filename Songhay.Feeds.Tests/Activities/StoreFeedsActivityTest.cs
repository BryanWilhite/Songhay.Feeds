using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using Songhay.Feeds.Activities;
using Songhay.Feeds.Activities.Extensions;
using Songhay.Feeds.Tests.Extensions;
using Songhay.Models;

namespace Songhay.Feeds.Tests.Activities
{
    [TestClass]
    public class StoreFeedsActivityTest
    {
        public TestContext TestContext { get; set; }

        [TestCategory("Integration")]
        [TestMethod]
        public void ShouldDisplayHelp()
        {
            var args = new[] { nameof(StoreFeedsActivity) };
            var activity = this.TestContext.ShouldGetActivityWithConfiguration(this.GetType(), args) as StoreFeedsActivity;
            Assert.IsNotNull(activity, "The expected activity is not here.");
            this.TestContext.WriteLine(activity.DisplayHelp(new ProgramArgs(args)));
        }

        [TestCategory("Integration")]
        [TestMethod]
        [TestProperty("cloudStorageSetName", "SonghayCloudStorage")]
        [TestProperty("connectionStringName", "general-purpose-v1")]
        public void ShouldGetCloudStorageAccount()
        {
            #region test properties:

            var cloudStorageSetName = this.TestContext.Properties["cloudStorageSetName"].ToString();
            var connectionStringName = this.TestContext.Properties["connectionStringName"].ToString();

            #endregion

            var args = new[] { nameof(StoreFeedsActivity) };
            var activity = this.TestContext.ShouldGetActivityWithConfiguration(this.GetType(), args) as StoreFeedsActivity;
            Assert.IsNotNull(activity, "The expected activity is not here.");

            var meta = activity.GetProgramMetadata();
            Assert.IsNotNull(meta, "The expected Program meta is not here.");

            var account = activity.GetCloudStorageAccount(meta, cloudStorageSetName, connectionStringName);
            Assert.IsNotNull(account, "The expected cloud storage account is not here.");
        }

        [TestCategory("Integration")]
        [TestMethod]
        [TestProperty("cloudStorageSetName", "SonghayCloudStorage")]
        [TestProperty("connectionStringName", "general-purpose-v1")]
        public void ShouldUploadJson()
        {
            var projectDirectoryInfo = this.TestContext.ShouldGetProjectDirectoryInfo(this.GetType());

            #region test properties:

            var cloudStorageSetName = this.TestContext.Properties["cloudStorageSetName"].ToString();
            var connectionStringName = this.TestContext.Properties["connectionStringName"].ToString();

            #endregion

            var args = new[] { nameof(StoreFeedsActivity), ProgramArgs.BasePath, projectDirectoryInfo.FullName };
            var activity = this.TestContext.ShouldGetActivityWithConfiguration(this.GetType(), args) as StoreFeedsActivity;
            Assert.IsNotNull(activity, "The expected activity is not here.");

            var meta = activity.GetProgramMetadata();
            Assert.IsNotNull(meta, "The expected Program meta is not here.");

            var account = activity.GetCloudStorageAccount(meta, cloudStorageSetName, connectionStringName);
            Assert.IsNotNull(account, "The expected cloud storage account is not here.");

            var feedsMeta = activity.Configuration.ToFeedsMetadata();

            activity.UploadJson(account, feedsMeta, new ProgramArgs(args));
        }
    }
}
