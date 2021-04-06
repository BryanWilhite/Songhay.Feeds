using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Songhay.Cloud.BlobStorage.Extensions;
using Songhay.Extensions;
using Songhay.Feeds.Activities;
using Songhay.Feeds.Activities.Extensions;
using Songhay.Feeds.Shell;
using Songhay.Feeds.Tests.Extensions;
using Songhay.Models;
using Songhay.Tests.Orderers;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Feeds.Tests
{
    public class ActivitiesTests
    {
        public ActivitiesTests(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
            this._projectPath = ProgramAssemblyUtility.GetPathFromAssembly(this.GetType().Assembly, "../../../");
        }

        [Trait("Category", "Integration")]
        [Fact, TestOrder(2)]
        public void ShouldConvertFeedsToJson()
        {
            var projectDirectoryInfo = new DirectoryInfo(this._projectPath);

            var args = new [] { nameof(DownloadFeedsActivity), ProgramArgs.BasePath, projectDirectoryInfo.FullName };
            var shellProjectDirectoryInfo = projectDirectoryInfo.Parent.GetDirectories().Single(i => i.Name.EndsWith("Shell"));
            var configuration = Shell.Program.LoadConfiguration(shellProjectDirectoryInfo.FullName);
            var activity = configuration.GetActivity(args) as DownloadFeedsActivity;
            Assert.NotNull(activity);

            var meta = activity.Configuration.ToFeedsMetadata();
            Assert.NotNull(meta);
            this._testOutputHelper.WriteLine(meta.ToString());

            using(var writer = new StringWriter())
            using(var listener = new TextWriterTraceListener(writer))
            {
                Program.InitializeTraceSource(listener, configuration);
                activity.ConvertFeedsToJson(meta, meta.ToRootDirectory(new ProgramArgs(args)));

                listener.Flush();
                this._testOutputHelper.WriteLine(writer.ToString());
            }
        }

        [Trait("Category", "Integration")]
        [Theory, InlineData(nameof(DownloadFeedsActivity)), InlineData(nameof(StoreFeedsActivity))]
        public void ShouldDisplayHelp(string activityName)
        {
            var projectDirectoryInfo = new DirectoryInfo(this._projectPath);

            var args = new [] { activityName };
            var shellProjectDirectoryInfo = projectDirectoryInfo.Parent.GetDirectories().Single(i => i.Name.EndsWith("Shell"));
            var configuration = Shell.Program.LoadConfiguration(shellProjectDirectoryInfo.FullName);

            var activity = configuration.GetActivity(args);

            switch (activityName)
            {
                case nameof(DownloadFeedsActivity):
                    Assert.NotNull(activity as DownloadFeedsActivity);
                    break;

                case nameof(StoreFeedsActivity):
                    Assert.NotNull(activity as StoreFeedsActivity);
                    break;

                default:
                    throw new NotImplementedException();
            }

            this._testOutputHelper.WriteLine(activity.DisplayHelp(new ProgramArgs(args)));
        }

        [Trait("Category", "Integration")]
        [Fact, TestOrder(1)]
        public void ShouldDownloadFeeds()
        {
            var projectDirectoryInfo = new DirectoryInfo(this._projectPath);

            var args = new [] { nameof(DownloadFeedsActivity), ProgramArgs.BasePath, projectDirectoryInfo.FullName };
            var shellProjectDirectoryInfo = projectDirectoryInfo.Parent.GetDirectories().Single(i => i.Name.EndsWith("Shell"));
            var configuration = Shell.Program.LoadConfiguration(shellProjectDirectoryInfo.FullName);
            var activity = configuration.GetActivity(args) as DownloadFeedsActivity;
            Assert.NotNull(activity);

            var meta = activity.Configuration.ToFeedsMetadata();
            Assert.NotNull(meta);
            this._testOutputHelper.WriteLine(meta.ToString());

            using(var writer = new StringWriter())
            using(var listener = new TextWriterTraceListener(writer))
            {
                Program.InitializeTraceSource(listener, configuration);

                activity.DownloadFeeds(meta, meta.ToRootDirectory(new ProgramArgs(args)));

                listener.Flush();
                this._testOutputHelper.WriteLine(writer.ToString());
            }
        }

        [Trait("Category", "Integration")]
        [Theory, InlineData("SonghayCloudStorage", "general-purpose-v1")]
        public void ShouldGetCloudStorageAccount(string cloudStorageSetName, string connectionStringName)
        {
            var projectDirectoryInfo = new DirectoryInfo(this._projectPath);

            var args = new [] { nameof(StoreFeedsActivity) };
            var shellProjectDirectoryInfo = projectDirectoryInfo.Parent.GetDirectories().Single(i => i.Name.EndsWith("Shell"));
            var configuration = Shell.Program.LoadConfiguration(shellProjectDirectoryInfo.FullName);
            var activity = configuration.GetActivity(args) as StoreFeedsActivity;
            Assert.NotNull(activity);

            var meta = activity.GetProgramMetadata();
            Assert.NotNull(meta);

            var account = meta.GetCloudStorageAccount(cloudStorageSetName, connectionStringName);
            Assert.NotNull(account);
        }

        [Trait("Category", "Integration")]
        [Fact]
        public void ShouldGetFeedsMetadata()
        {
            var projectDirectoryInfo = new DirectoryInfo(this._projectPath);

            var args = new [] { nameof(DownloadFeedsActivity) };
            var shellProjectDirectoryInfo = projectDirectoryInfo.Parent.GetDirectories().Single(i => i.Name.EndsWith("Shell"));
            var configuration = Shell.Program.LoadConfiguration(shellProjectDirectoryInfo.FullName);
            var activity = configuration.GetActivity(args) as DownloadFeedsActivity;
            Assert.NotNull(activity);

            var meta = activity.Configuration.ToFeedsMetadata();
            Assert.NotNull(meta);
            this._testOutputHelper.WriteLine(meta.ToString());
        }

        [Trait("Category", "Integration")]
        [Fact]
        public void ShouldGetRootDirectory()
        {
            var projectDirectoryInfo = new DirectoryInfo(this._projectPath);

            var args = new [] { nameof(DownloadFeedsActivity) };
            var shellProjectDirectoryInfo = projectDirectoryInfo.Parent.GetDirectories().Single(i => i.Name.EndsWith("Shell"));
            var configuration = Shell.Program.LoadConfiguration(shellProjectDirectoryInfo.FullName);
            var activity = configuration.GetActivity(args) as DownloadFeedsActivity;
            Assert.NotNull(activity);

            var meta = activity.Configuration.ToFeedsMetadata();
            Assert.NotNull(meta);
            this._testOutputHelper.WriteLine(meta.ToString());

            var root = meta.ToRootDirectory(new ProgramArgs(args));
            this._testOutputHelper.WriteLine(root);
        }

        [Trait("Category", "Integration")]
        [Theory, TestOrder(2), InlineData("SonghayCloudStorage", "general-purpose-v1")]
        public void ShouldUploadJson(string cloudStorageSetName, string connectionStringName)
        {
            var projectDirectoryInfo = new DirectoryInfo(this._projectPath);

            var args = new [] { nameof(StoreFeedsActivity) };
            var shellProjectDirectoryInfo = projectDirectoryInfo.Parent.GetDirectories().Single(i => i.Name.EndsWith("Shell"));
            var configuration = Shell.Program.LoadConfiguration(shellProjectDirectoryInfo.FullName);
            var activity = configuration.GetActivity(args) as StoreFeedsActivity;
            Assert.NotNull(activity);

            var meta = activity.GetProgramMetadata();
            Assert.NotNull(meta);

            var account = meta.GetCloudStorageAccount(cloudStorageSetName, connectionStringName);
            Assert.NotNull(account);

            var feedsMeta = activity.Configuration.ToFeedsMetadata();
            feedsMeta.FeedsDirectory = projectDirectoryInfo.ToCombinedPath(feedsMeta.FeedsDirectory);

            using(var writer = new StringWriter())
            using(var listener = new TextWriterTraceListener(writer))
            {
                Program.InitializeTraceSource(listener, configuration);

                activity.UploadJson(account, feedsMeta, new ProgramArgs(args));

                listener.Flush();
                this._testOutputHelper.WriteLine(writer.ToString());
            }
        }

        readonly string _projectPath;
        readonly ITestOutputHelper _testOutputHelper;
    }
}