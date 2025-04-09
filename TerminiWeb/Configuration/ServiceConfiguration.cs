using AutoMapper;
using Microsoft.Extensions.Options;
using TerminiWeb.Infrastructure.Common.Configuration;
using TerminiWeb.Infrastructure.PlayerService;
using TerminiWeb.Infrastructure.TerminService;
using TerminiWeb.Infrastructure.WeatherService;

namespace TerminiWeb.Configuration
{
	public static class ServiceConfiguration
	{
		public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services
				.ConfigureAppSettings(configuration)
				.ConfigureAutoMapper()
				.ConfigureApplicationServices()
				.ConfigureHttpClients(configuration);

			// Authentication is currently commented out but kept as a separate method
			ConfigureAuthentication(services, configuration);
		}

		private static IServiceCollection ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
		{
			// Application settings configuration
			services.Configure<TerminiAppSettings>(
				configuration.GetSection(nameof(TerminiAppSettings)));

			services.Configure<ApiEndpointSettings>(
				configuration.GetSection(nameof(ApiEndpointSettings)));

			return services;
		}

		private static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
		{
			var mappingConfig = new MapperConfiguration(mc =>
			{
				// Date mapping configurations
				mc.CreateMap<DateOnly, DateTime>()
					.ConvertUsing(x => x.ToDateTime(TimeOnly.MinValue));
				mc.CreateMap<DateTime, DateOnly>()
					.ConvertUsing(x => DateOnly.FromDateTime(x));

				mc.CreateMap<DateOnly?, DateTime?>()
					.ConvertUsing(x => x != null ? x.Value.ToDateTime(TimeOnly.MinValue) : null);
				mc.CreateMap<DateTime?, DateOnly?>()
					.ConvertUsing(x => x != null ? DateOnly.FromDateTime(x.Value) : null);
			});

			services.AddSingleton(mappingConfig.CreateMapper());
			return services;
		}

		private static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
		{
			// Register application services
			services.AddTransient<IWeatherService, WeatherService>();
			services.AddTransient<IPlayerService, PlayerService>();
			services.AddTransient<ITerminService, TerminService>();

			return services;
		}

		private static IServiceCollection ConfigureHttpClients(this IServiceCollection services, IConfiguration configuration)
		{
			// Configure HTTP clients with common base URL
			ConfigureHttpClientWithBaseUrl<WeatherService>(services);
			ConfigureHttpClientWithBaseUrl<PlayerService>(services);

			return services;
		}

		private static void ConfigureHttpClientWithBaseUrl<TClient>(IServiceCollection services) where TClient : class
		{
			services.AddHttpClient<TClient>()
				.ConfigureHttpClient((serviceProvider, client) =>
				{
					var apiEndpointSettings = serviceProvider
						.GetRequiredService<IOptions<ApiEndpointSettings>>().Value;
					client.BaseAddress = new Uri(apiEndpointSettings.TerminiApiBaseUrl);
				});
		}

		private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
		{
			// Authentication configuration is commented out but preserved for future use
			//services
			//    .AddKeycloakAuthenticationSettings()
			//    .AddAuthentication(options =>
			//    {
			//        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			//        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			//    })
			//    .AddKeycloakAuthenticationPolicy()
			//    .AddKeycloakCeresAuthenticationSchemes(configuration);
			//services.AddCeresAuthentication();
		}
	}
}
