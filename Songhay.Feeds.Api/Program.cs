using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;

using Songhay.Web.Handlers;
using Songhay.Web.HealthChecks;
using Songhay.Web.Models;

WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddAuthentication(ApiKeyAuthenticationOptions.DefaultScheme)
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
        ApiKeyAuthenticationOptions.DefaultScheme, _ => { });

builder.Services.AddAuthorization();
builder.Services.AddOutputCache();

builder.Services.AddHealthChecks()
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

// Readiness: run only checks tagged "ready"
app
    .MapHealthChecks(
        $"api/{HealthCheckConstants.ReadinessRoute}",
        new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains(HealthCheckConstants.Ready),
            AllowCachingResponses = true
        })
        .CacheOutput(policy => policy.Expire(TimeSpan.FromSeconds(5)))
        .RequireAuthorization();

// Liveness: run NO checks. A 200 just means the process can serve a request.
app
    .MapHealthChecks(
        $"/{HealthCheckConstants.LivenessRoute}",
        new HealthCheckOptions
        {
            Predicate = _ => false
        });

app.Run();
