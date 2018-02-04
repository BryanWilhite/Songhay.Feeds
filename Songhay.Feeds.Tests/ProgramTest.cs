using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Feeds.Tests.Extensions;

namespace Songhay.Feeds.Tests
{
    [TestClass]
    public class ProgramTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ShouldLoadConfiguration()
        {
            var shellProjectDirectoryInfo = this.TestContext.ShouldGetShellProjectDirectoryInfo(this.GetType());
            var configuration = Shell.Program.LoadConfiguration(shellProjectDirectoryInfo.FullName);
            Assert.IsNotNull(configuration);
        }

        [TestMethod]
        [TestProperty("feedsDirectory", "feeds")]
        public void ShouldLoadFeedsMetadata()
        {
            //meta.Feeds.ForEachInEnumerable(feed =>
            //{
            //    var uri = new Uri(feed.Value, UriKind.Absolute);
            //    this.TestContext.WriteLine($"uri: {uri.OriginalString}");
            //    var client = new HttpClient();
            //    var request = new HttpRequestMessage() { RequestUri = uri, Method = HttpMethod.Get };

            //    var response = client.SendAsync(request).Result;
            //    Assert.IsNotNull(response, "The expected response is not here.");
            //    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "The expected response status code is not here.");

            //    var content = response.Content.ReadAsStringAsync().Result;
            //    File.WriteAllText(Path.Combine(feedsDirectory, $"{feed.Key}.xml"), content);
            //});
        }
    }
}
