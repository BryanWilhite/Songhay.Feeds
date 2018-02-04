using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using System;
using System.IO;
using System.Linq;

namespace Songhay.Feeds.Tests.Extensions
{
    public static class TestContextExtensions
    {
        public static DirectoryInfo ShouldGetShellProjectDirectoryInfo(this TestContext context, Type type)
        {

            var projectDirectoryInfo = context.ShouldGetTestProjectDirectoryInfo(type);
            var shellProjectDirectoryInfo = projectDirectoryInfo.Parent.GetDirectories().Single(i => i.Name.EndsWith("Shell"));
            Assert.IsNotNull(shellProjectDirectoryInfo, "The expected Shell project directory is not here.");

            return shellProjectDirectoryInfo;
        }

        public static DirectoryInfo ShouldGetTestProjectDirectoryInfo(this TestContext context, Type type)
        {
            var projectDirectoryInfo = context
                            .ShouldGetAssemblyDirectoryInfo(type) // netcoreapp2.0
                            ?.Parent // Debug or Release
                            ?.Parent // bin
                            ?.Parent;
            context.ShouldFindDirectory(projectDirectoryInfo?.FullName);

            return projectDirectoryInfo;
        }
    }
}
