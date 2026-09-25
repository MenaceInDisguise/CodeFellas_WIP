var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerfile(
    "Prosjekt",
    "..",
    "Prosjekt/Dockerfile")
    .WithHttpEndpoint(
        port: 5027,
        targetPort: 8080);

builder.Build().Run();
