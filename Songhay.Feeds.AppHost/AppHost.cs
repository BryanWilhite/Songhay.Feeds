var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Songhay_Feeds_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.Build().Run();
