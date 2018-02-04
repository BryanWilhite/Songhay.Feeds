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
            Assert.IsNotNull(configuration, "The expected configuration is not here.");
        }
    }
}
