using TerminiAPI;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddCors(options =>
{
	options.AddPolicy("ClientPermission", policy =>
	{
		policy.AllowAnyHeader()
			.AllowAnyMethod()
			.SetIsOriginAllowed(_ => true)
			.AllowCredentials();
	});
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseCors("ClientPermission");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
