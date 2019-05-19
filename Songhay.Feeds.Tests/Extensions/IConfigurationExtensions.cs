using Microsoft.Extensions.Configuration;
using Songhay.Extensions;
using Songhay.Models;
using Xunit;

namespace Songhay.Feeds.Tests.Extensions
{
    public static class IConfigurationExtensions
    {
        public static IActivity GetActivity(this IConfigurationRoot configuration, string[] args)
        {
            Assert.NotNull(configuration);

            var getter = Shell.Program.GetActivitiesGetter(args);
            Assert.NotNull(getter);

            var activity = getter.GetActivity().WithConfiguration(configuration);
            Assert.NotNull(activity);

            return activity;
        }
    }
}
