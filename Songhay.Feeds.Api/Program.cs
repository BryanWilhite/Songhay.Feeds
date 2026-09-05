using Scalar.AspNetCore;

using Songhay.Extensions;
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
ArgumentNullException.ThrowIfNull(programMetadata);

WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);

RestApiMetadata restApiMetadata = programMetadata
    .ToRestApiMetadata("songhay-feeds-api");

ApiUriSet uriHealthCheckSet = restApiMetadata
    .ToApiUriSetFromClaimSetByPrefix("feed-");

builder.Services
    .AddSingleton(programMetadata)
    .AddSingleton(uriHealthCheckSet)
    .AddRestApiMetadataForApiKey(restApiMetadata)
    .AddActivityGroup<AmazonS3ActivityGroup>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddAuthentication(ApiKeyAuthenticationOptions.DefaultScheme)
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
        ApiKeyAuthenticationOptions.DefaultScheme, _ => { });

builder.Services
    .AddAuthorization()
    .AddOutputCache()
    .AddHealthChecks()
    .AddApplicationLifecycleHealthCheck(
        tags: [HealthCheckConstants.Ready]
    )
    .AddResourceUtilizationHealthCheck()
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

app.MapHealthChecks(
    $"api/{HealthCheckConstants.ReadinessRoute}",
    HealthCheckUtility
        .GetHealthCheckOptionsWithFiltering(cr => cr.Tags.Contains(HealthCheckConstants.Ready))
        .WithClientCachingAllowed())
    .CacheOutput(policy => policy.Expire(TimeSpan.FromSeconds(5)))
    .RequireAuthorization();

app.MapHealthChecks(
    $"/{HealthCheckConstants.LivenessRoute}",
    HealthCheckUtility
        .GetHealthCheckOptionsForZeroChecks());

app.Run();
