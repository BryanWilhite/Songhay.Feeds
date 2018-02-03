using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Songhay.Extensions;
using Songhay.Feeds.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Songhay.Feeds.Tests
{
    [TestClass]
    public class AppSettingsTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [TestProperty("feedsDirectory", "feeds")]
        public void ShouldLoadFeedsMetadata()
        {

            var projectDirectoryInfo = this.TestContext
                            .ShouldGetAssemblyDirectoryInfo(this.GetType())
                            ?.Parent // netcoreapp2.0
                            ?.Parent // Debug or Release
                            ?.Parent // bin
                            ?.Parent;
            this.TestContext.ShouldFindDirectory(projectDirectoryInfo?.FullName);

            var testProjectDirectory = projectDirectoryInfo.FullName;

            var shellProjectDirectoryInfo = projectDirectoryInfo.Parent.GetDirectories().Single(i => i.Name.EndsWith("Shell"));
            Assert.IsNotNull(shellProjectDirectoryInfo, "The expected Shell project directory is not here.");

            var shellProjectDirectory = shellProjectDirectoryInfo.FullName;

            #region test properties:

            var feedsDirectory = this.TestContext.Properties["feedsDirectory"].ToString();
            feedsDirectory = Path.Combine(testProjectDirectory, feedsDirectory);
            this.TestContext.ShouldFindDirectory(feedsDirectory);

            #endregion

            var settingsFile = Path.Combine(shellProjectDirectory, "appsettings.json");
            this.TestContext.ShouldFindFile(settingsFile);

            var settingsJSON = File.ReadAllText(settingsFile);
            var settingsJO = JObject.Parse(settingsJSON);

            var meta = settingsJO[nameof(FeedsMetadata)].ToObject<FeedsMetadata>();
            Assert.IsNotNull(meta, "The expected meta data is not here.");

            meta.Feeds.ForEachInEnumerable(feed =>
            {
                var uri = new Uri(feed.Value, UriKind.Absolute);
                this.TestContext.WriteLine($"uri: {uri.OriginalString}");
                var client = new HttpClient();
                var request = new HttpRequestMessage() { RequestUri = uri, Method = HttpMethod.Get };

                var response = client.SendAsync(request).Result;
                Assert.IsNotNull(response, "The expected response is not here.");
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The expected response status code is not here.");

                var content = response.Content.ReadAsStringAsync().Result;
                File.WriteAllText(Path.Combine(feedsDirectory, $"{feed.Key}.xml"), content);
            });
        }
    }
}
