var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.TerminiAPI>("terminiapi");
builder.AddProject<Projects.TerminiWeb>("terminiweb");

builder.Build().Run();
