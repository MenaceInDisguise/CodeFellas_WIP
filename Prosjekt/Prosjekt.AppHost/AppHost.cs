var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Prosjekt>("prosjekt");

builder.Build().Run();
