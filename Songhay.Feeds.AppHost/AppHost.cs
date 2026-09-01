var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("env");

var apiService = builder.AddProject<Projects.Songhay_Feeds_ApiService>("apiservice")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health");

builder.Build().Run();
