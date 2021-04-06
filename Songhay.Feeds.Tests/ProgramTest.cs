using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Songhay.Feeds.Tests
{
    public class ProgramTest
    {
        public ProgramTest(ITestOutputHelper helper)
        {
            this._testOutputHelper = helper;
            this._projectPath = ProgramAssemblyUtility.GetPathFromAssembly(this.GetType().Assembly, "../../../");
        }

        [Trait("Category", "Integration")]
        [Fact]
        public void ShouldLoadConfiguration()
        {
            var projectDirectoryInfo = new DirectoryInfo(this._projectPath);
            var shellProjectDirectoryInfo = projectDirectoryInfo.Parent.GetDirectories().Single(i => i.Name.EndsWith("Shell"));

            Assert.NotNull(shellProjectDirectoryInfo);
            var configuration = Shell.Program.LoadConfiguration(shellProjectDirectoryInfo.FullName);
            Assert.NotNull(configuration);
        }

        readonly string _projectPath;
        readonly ITestOutputHelper _testOutputHelper;
    }
}