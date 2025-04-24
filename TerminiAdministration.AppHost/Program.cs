var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.TerminiAPI>("terminiapi");
builder.AddProject<Projects.TerminiWeb>("terminiweb")
	.WithReference(api)
	.WaitFor(api);

builder.Build().Run();
