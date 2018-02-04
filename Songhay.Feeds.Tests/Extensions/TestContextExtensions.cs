using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using Songhay.Feeds.Activities;
using Songhay.Feeds.Models;
using Songhay.Models;
using System;
using System.IO;
using System.Linq;

namespace Songhay.Feeds.Tests.Extensions
{
    public static class TestContextExtensions
    {
        public static IActivity ShouldGetActivityWithConfiguration(this TestContext context, Type type, string[] args)
        {
            var shellProjectDirectoryInfo = context.ShouldGetShellProjectDirectoryInfo(type);
            var configuration = Shell.Program.LoadConfiguration(shellProjectDirectoryInfo.FullName);
            Assert.IsNotNull(configuration, "The expected configuration is not here.");

            var getter = Shell.Program.GetActivitiesGetter(args);
            Assert.IsNotNull(getter, "The expected activities getter is not here.");

            var activity = getter.GetActivity().WithConfiguration(configuration);
            Assert.IsNotNull(activity, "The expected activity is not here.");

            return activity;
        }

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
