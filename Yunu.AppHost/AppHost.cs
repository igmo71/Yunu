var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Yunu_Api>("yunu-api");

builder.Build().Run();
