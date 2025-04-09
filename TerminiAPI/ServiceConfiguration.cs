using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TerminiDataAccess.TerminiContext;
using TerminiService.Common.Configuration;
using TerminiService.PlayerService;
using TerminiService.TerminService;
using TerminiService.WeatherService;

namespace TerminiAPI
{
	public static class ServiceConfiguration
	{
		public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddApplicationServices();
			services.AddDatabaseContext(configuration);
			services.AddAutoMapper();
			services.AddConfiguration(configuration);
		}

		private static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddTransient<IWeatherService, WeatherService>();
			services.AddTransient<IPlayerService, PlayerService>();
			services.AddTransient<ITerminService, TerminService>();

			return services;
		}

		private static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<TerminiContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("Default"));
#if !DEBUG
                options.UseModel(TerminiContextModel.Instance);
#endif
				options.EnableSensitiveDataLogging();
			});

			return services;
		}

		private static IServiceCollection AddAutoMapper(this IServiceCollection services)
		{
			var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.CreateMap<DateOnly, DateTime>().ConvertUsing(x => x.ToDateTime(TimeOnly.MinValue));
				mc.CreateMap<DateTime, DateOnly>().ConvertUsing(x => DateOnly.FromDateTime(x));

				mc.CreateMap<DateOnly?, DateTime?>().ConvertUsing(x => x != null ? x.Value.ToDateTime(TimeOnly.MinValue) : null);
				mc.CreateMap<DateTime?, DateOnly?>().ConvertUsing(x => x != null ? DateOnly.FromDateTime(x.Value) : null);
			});

			services.AddSingleton(mappingConfig.CreateMapper());
			return services;
		}

		private static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<TerminiApiAppSettings>(configuration.GetSection(nameof(TerminiApiAppSettings)));
			return services;
		}
	}
}
