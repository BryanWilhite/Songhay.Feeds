# Songhay Feeds

Songhay Feeds are `Activities` around downloading Syndication feeds (in RSS or Atom) and converting them into static JSON files. This file format can be used to bind to display component for a “dashboard” view.

`Songhay.Feeds` is the smallest back-end workload of this Studio, representing a move toward the following conventions:

- using [[ASP.NET]] minimal Web API with <acronym title="ahead of time">AOT</acronym> compilation [📖 [docs](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/?tabs=windows%2Cnet8) ] for the smallest possible, fastest, container-friendly releases (currently about 100MB which is embarrassingly gigantic compared to equivalent [[Go]]-based containers—I get it now 🤦)
- using custom, domain-specific health checks on top of the [standard health checks](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/monitor-app-health)
- starting with the new `ApiKeyAuthenticationHandler`, inspired by [guidance](https://codewithmukesh.com/blog/api-key-authentication-aspnet-core/) from Mukesh Murugan
- using [[Quartz.NET]] in volatile memory (and, in future, with persistent storage)
- Leveraging [Songhay Activities](https://github.com/BryanWilhite/SonghayCore#the-core-activity-concept), designed for any `IHost`-based environment, clearly defining inputs and outputs for “business logic”
- using [[Podman]] in the Studio
- using [[MSBuild]] properties exclusively in .NET project files, including container-specific properties [📖 [docs](https://learn.microsoft.com/en-us/dotnet/core/containers/publish-configuration) ]

[Bryan Wilhite is on LinkedIn](https://www.linkedin.com/in/wilhite)🇺🇸💼
