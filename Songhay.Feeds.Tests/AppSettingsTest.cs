using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Songhay.Extensions;
using Songhay.Feeds.Models;
using Songhay.Feeds.Tests.Extensions;
using System;
using System.IO;
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
        public void ShouldLoadFeedsControllerMetadata()
        {

            var projectDirectory = this.TestContext
                .ShouldGetAssemblyDirectoryInfo(this.GetType()) // netcoreapp2.0
                ?.Parent // Debug
                ?.Parent // bin
                ?.Parent.FullName;
            this.TestContext.ShouldFindDirectory(projectDirectory);

            #region test properties:

            var feedsDirectory = this.TestContext.Properties["feedsDirectory"].ToString();
            feedsDirectory = Path.Combine(projectDirectory, feedsDirectory);
            this.TestContext.ShouldFindDirectory(feedsDirectory);

            #endregion

            var basePath = this.TestContext.ShouldGetBasePath(this.GetType());
            var settingsFile = Path.Combine(basePath, "appsettings.json");
            this.TestContext.ShouldFindFile(settingsFile);

            var settingsJSON = File.ReadAllText(settingsFile);
            var settingsJO = JObject.Parse(settingsJSON);

            var meta = settingsJO[nameof(FeedsControllerMetadata)].ToObject<FeedsControllerMetadata>();
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
