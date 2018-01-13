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

        TestServer _server;
    }
}
