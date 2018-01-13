using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using Songhay.Models;
using Songhay.Feeds.Tests.Extensions;
using System.IO;
using System.Linq;
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
            var projectsDirectory = this.TestContext
                            .ShouldGetAssemblyDirectoryInfo(this.GetType())
                            ?.Parent
                            ?.Parent
                            ?.Parent
                            ?.Parent.FullName;
            this.TestContext.ShouldFindDirectory(projectsDirectory);

            var targetProjectName = string.Join('.', this.GetType().Namespace.Split('.').Take(2));

            var basePath = Path.Combine(projectsDirectory, targetProjectName);
            var builder = Program.GetWebHostBuilder(new[] { ProgramArgs.BasePath, basePath });
            this._server = new TestServer(builder);
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
