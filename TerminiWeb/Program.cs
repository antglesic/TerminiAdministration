using System.Diagnostics;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Serilog;
using TerminiWeb;
using TerminiWeb.Configuration;
using TerminiWeb.Infrastructure.Common.Configuration;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register ApiEndpointSettings in DI
builder.Services.AddSingleton<ApiEndpointSettings>(provider =>
{
	var configuration = provider.GetRequiredService<IConfiguration>();
	var apiEndpointSettings = new ApiEndpointSettings();
	configuration.GetSection("ApiEndpointSettings").Bind(apiEndpointSettings);
	return apiEndpointSettings;
});

// Set up logging (as you already have)
Log.Logger = new LoggerConfiguration()
			.Enrich.FromLogContext()
			.CreateLogger();

Serilog.Debugging.SelfLog.Enable(msg =>
{
	Debug.Print(msg);
	Debugger.Break();
});

// Register additional services
builder.Services.AddHttpClient();
builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMudServices(config =>
{
	config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;
	config.SnackbarConfiguration.PreventDuplicates = false;
	config.SnackbarConfiguration.NewestOnTop = false;
	config.SnackbarConfiguration.ShowCloseIcon = true;
	config.SnackbarConfiguration.ClearAfterNavigation = true;
	config.SnackbarConfiguration.VisibleStateDuration = 1000;
	config.SnackbarConfiguration.HideTransitionDuration = 500;
	config.SnackbarConfiguration.ShowTransitionDuration = 500;
	config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

await builder.Build().RunAsync();
