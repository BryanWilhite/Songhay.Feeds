using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Feeds.Tests.Extensions;
using System.IO;
using System.Threading.Tasks;
using Tavis.UriTemplates;

namespace Songhay.Feeds.Tests.Controllers
{
    [TestClass]
    public class SyndicationControllerTest
    {
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void InitializeTest()
        {
            var basePath = this.TestContext.ShouldGetBasePath(this.GetType());
            var builder = Program.GetWebHostBuilder();
            Assert.IsNotNull(builder, "The expected Web Host builder is not here.");

            builder.ConfigureAppConfiguration((builderContext, configBuilder) =>
            {
                Assert.IsNotNull(builderContext, "The expected Web Host builder context is not here.");

                var env = builderContext.HostingEnvironment;
                Assert.IsNotNull(env, "The expected Hosting Environment is not here.");

                env.ContentRootPath = basePath;
                env.EnvironmentName = "Development";
                env.WebRootPath = $"{basePath}{Path.DirectorySeparatorChar}wwwroot";
            });

            this._server = new TestServer(builder);
            Assert.IsNotNull(this._server, "The expected test server is not here.");
        }

        [TestMethod]
        [TestProperty("pathTemplate", "api/syndication/info/{feed}")]
        [TestProperty("feed", "studio")]
        public async Task ShouldHaveFeed()
        {
            #region test properties:

            var pathTemplate = new UriTemplate(this.TestContext.Properties["pathTemplate"].ToString());
            var feed = this.TestContext.Properties["feed"].ToString();

            #endregion

            var path = pathTemplate.BindByPosition(feed);
            var client = this._server.CreateClient();
            var response = await client.GetAsync(path);

            var content = response.Content.ReadAsStringAsync().Result;
            if (string.IsNullOrEmpty(content)) Assert.Fail("The expected content in the response is not here.");

            this.TestContext.WriteLine("raw content: {0}", content);

            response.EnsureSuccessStatusCode();
        }

        TestServer _server;
    }
}
