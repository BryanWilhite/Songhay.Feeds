using Quartz;
using Scalar.AspNetCore;

using Songhay.Abstractions;
using Songhay.Extensions;
using Songhay.Feeds.Activities;
using Songhay.Feeds.Api.Extensions;
using Songhay.Feeds.Api.Jobs;
using Songhay.Models;
using Songhay.S3.Extensions;
using Songhay.S3.Models;
using Songhay.Web;
using Songhay.Web.Extensions;
using Songhay.Web.Handlers;
using Songhay.Web.HealthChecks;
using Songhay.Web.Models;

ProgramMetadata? programMetadata = ProgramMetadataUtility
    .GetProgramMetadataFromEnvironment();

programMetadata.EnsureProgramMetadata();

WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);

builder.WebHost.UseKestrelHttpsConfiguration();

RestApiMetadata restApiMetadata = programMetadata
    .ToRestApiMetadata("SonghayFeedsApi");

ApiUriSet uriHealthCheckSet = restApiMetadata
    .ToApiUriSetFromClaimSetByPrefix("feed-");

builder.Services
    .AddAuthorization()
    .AddOutputCache()
    .AddHttpClient()
    .AddSingleton(programMetadata)
    .AddSingleton(uriHealthCheckSet)
    .AddRestApiMetadataForApiKey(restApiMetadata)
    .AddActivityGroup<AmazonS3ActivityGroup>()
    .AddKeyedTransient<IActivityTask, FeedDownloadActivity>(nameof(FeedDownloadActivity))
    .AddKeyedTransient<IJob, FeedDownloadJob>(nameof(FeedDownloadJob))
    .AddQuartz(qBuilder =>
    {
        JobKey key = new(nameof(FeedDownloadJob));
        qBuilder.AddJob<FeedDownloadJob>(options => options.WithIdentity(key));

        qBuilder.AddTrigger(options => options
            .ForJob(key)
            .WithIdentity($"{nameof(FeedDownloadJob)}TriggerKey")
            .WithSimpleSchedule(schedule => schedule.WithConventions(restApiMetadata)));
    })
    .AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// returns AuthenticationBuilder:
builder.Services
    .AddAuthentication(ApiKeyAuthenticationOptions.DefaultScheme)
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
        ApiKeyAuthenticationOptions.DefaultScheme, _ => { });

// returns IHealthChecksBuilder:
builder.Services
    .AddHealthChecks()
    .AddApplicationLifecycleHealthCheck(
        tags: [HealthCheckConstants.Ready]
    )
    .AddCheck<UriHealthCheck>(
        name: UriHealthCheck.Name,
        tags: [HealthCheckConstants.Ready]
    );

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapHealthChecks(
    $"api/{HealthCheckConstants.ReadinessRoute}",
    HealthCheckUtility
        .GetHealthCheckOptionsWithFiltering(cr => cr.Tags
            .Contains(HealthCheckConstants.Ready))
        .WithClientCachingAllowed())
        .CacheOutput(policy => policy.Expire(TimeSpan.FromSeconds(5)))
        .RequireAuthorization();

app.MapHealthChecks(
    $"/{HealthCheckConstants.LivenessRoute}",
    HealthCheckUtility
        .GetHealthCheckOptionsForZeroChecks());

app.Run();
