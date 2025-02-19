using AutoMapper;
using Microsoft.Extensions.Options;
using TerminiWeb.Infrastructure.Common.Configuration;
using TerminiWeb.Infrastructure.WeatherService;

namespace TerminiWeb.Configuration
{
	public static class ServiceConfiguration
	{
		public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			ConfigureApplicationServices(services, configuration);
			ConfigureAuthentication(services, configuration);
		}

		private static void ConfigureApplicationServices(IServiceCollection services, IConfiguration configuration)
		{
			// Add settings
			services.Configure<TerminiAppSettings>(options =>
			{
				configuration.GetSection(nameof(TerminiAppSettings)).Bind(options);
			});
			services.Configure<ApiEndpointSettings>(options =>
			{
				configuration.GetSection(nameof(ApiEndpointSettings)).Bind(options);
			});

			// Add mappers
			MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
			{
				mc.CreateMap<DateOnly, DateTime>().ConvertUsing(x => x.ToDateTime(TimeOnly.MinValue));
				mc.CreateMap<DateTime, DateOnly>().ConvertUsing(x => DateOnly.FromDateTime(x));

				mc.CreateMap<DateOnly?, DateTime?>().ConvertUsing(x => x != null ? x.Value.ToDateTime(TimeOnly.MinValue) : null);
				mc.CreateMap<DateTime?, DateOnly?>().ConvertUsing(x => x != null ? DateOnly.FromDateTime(x.Value) : null);
			});

			services.AddSingleton(mappingConfig.CreateMapper());

			// Add services
			services.AddTransient<IWeatherService, WeatherService>();

			// Configure Service HttpClient for external services
			services.AddHttpClient<WeatherService>()
				.ConfigureHttpClient((serviceProvider, client) =>
				{
					ApiEndpointSettings apiEndpointSettings = serviceProvider.GetRequiredService<IOptions<ApiEndpointSettings>>().Value;
					client.BaseAddress = new Uri(apiEndpointSettings.TerminiApiBaseUrl);
				});
		}

		private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
		{
			//services
			//	.AddKeycloakAuthenticationSettings()
			//	.AddAuthentication(options =>
			//	{
			//		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			//		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			//	})
			//	.AddKeycloakAuthenticationPolicy()
			//	.AddKeycloakCeresAuthenticationSchemes(configuration);
			//services.AddCeresAuthentication();
		}
	}
}
