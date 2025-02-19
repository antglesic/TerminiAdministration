using TerminiService.Common.Configuration;
using TerminiService.WeatherService;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add settings
builder.Services.Configure<TerminiApiAppSettings>(options =>
{
	builder.Configuration.GetSection(nameof(TerminiApiAppSettings)).Bind(options);
});

// Add services
builder.Services.AddTransient<IWeatherService, WeatherService>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


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
