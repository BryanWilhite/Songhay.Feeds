using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Songhay.Feeds.Tests")]

namespace Songhay.Feeds
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = GetWebHostBuilder(args);
            builder.Build().Run();
        }

        internal static IWebHostBuilder GetWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }
}
