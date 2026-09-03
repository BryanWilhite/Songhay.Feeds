using Microsoft.Extensions.Diagnostics.HealthChecks;

using Songhay.Web.Models;

namespace Songhay.Web.HealthChecks;

/// <summary>
/// Implements <see cref="IHealthCheck"/>
/// to make HEAD requests with the injected <see cref="UriHealthCheckSet"/>.
/// </summary>
public sealed class UriHealthCheck(IHttpClientFactory httpClientFactory, UriHealthCheckSet uriHealthCheckSet) : IHealthCheck
{
    public const string Name = "uri-health-check";

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
