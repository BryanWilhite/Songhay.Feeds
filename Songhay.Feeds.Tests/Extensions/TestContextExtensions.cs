using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using System;
using System.IO;
using System.Linq;

namespace Songhay.Feeds.Tests.Extensions
{
    public static class TestContextExtensions
    {
        public static string ShouldGetBasePath(this TestContext context, Type type)
        {
            var projectsDirectory = context
                            .ShouldGetAssemblyDirectoryInfo(type)
                            ?.Parent // netcoreapp2.0
                            ?.Parent // Debug or Release
                            ?.Parent // bin
                            ?.Parent.FullName;
            context.ShouldFindDirectory(projectsDirectory);

            var targetProjectName = string.Join('.', type.Namespace.Split('.').Take(2));

            var basePath = Path.Combine(projectsDirectory, targetProjectName);

            context.ShouldFindDirectory(basePath);

            return basePath;
        }
    }
}
