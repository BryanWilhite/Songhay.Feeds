using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Songhay.Extensions;
using Songhay.Models;
using System;
using System.IO;
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

        internal static IWebHostBuilder GetWebHostBuilder(string[] args) => WebHost
                    .CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration(configureDelegate: (builderContext, config) =>
                    {
                        if (args == null) return;

                        var proArgs = new ProgramArgs(args);

                        config.AddCommandLine(args);
                        if (proArgs.HasArg(ProgramArgs.BasePath, requiresValue: true))
                        {
                            var basePath = proArgs.GetArgValue(ProgramArgs.BasePath);
                            if (!Directory.Exists(basePath)) throw new ArgumentException($"{basePath} does not exist.");
                            config.SetBasePath(basePath);
                        }
                    })
                    .UseStartup<Startup>();
    }
}
