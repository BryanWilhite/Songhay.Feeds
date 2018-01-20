using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Songhay.Extensions;
using Songhay.Models;
using System;
using System.IO;

namespace Songhay.Feeds
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                ;
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        internal static void HandleAppConfiguration(string[] args, WebHostBuilderContext context, IConfigurationBuilder configurationBuilder)
        {
            if (args == null) return;

            var proArgs = new ProgramArgs(args);

            configurationBuilder.AddCommandLine(args);
            if (proArgs.HasArg(ProgramArgs.BasePath, requiresValue: true))
            {
                var basePath = proArgs.GetArgValue(ProgramArgs.BasePath);
                if (!Directory.Exists(basePath)) throw new ArgumentException($"{basePath} does not exist.");
                configurationBuilder.SetBasePath(basePath);
            }
        }
    }
}
