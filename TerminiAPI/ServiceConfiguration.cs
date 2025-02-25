using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TerminiDataAccess.TerminiContext;
using TerminiService.Common.Configuration;
using TerminiService.WeatherService;

namespace TerminiAPI
{
	public static class ServiceConfiguration
	{
		public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			ConfigureApplicationServices(services, configuration);
		}

		private static void ConfigureApplicationServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddTransient<IWeatherService, WeatherService>();

			services.AddDbContext<TerminiContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("Default"));
#if !DEBUG
				options.UseModel(TerminiContextModel.Instance);
#endif
				options.EnableSensitiveDataLogging();
			});

			MapperConfiguration mappingConfig = new MapperConfiguration(mc =>
			{
				mc.CreateMap<DateOnly, DateTime>().ConvertUsing(x => x.ToDateTime(TimeOnly.MinValue));
				mc.CreateMap<DateTime, DateOnly>().ConvertUsing(x => DateOnly.FromDateTime(x));

				mc.CreateMap<DateOnly?, DateTime?>().ConvertUsing(x => x != null ? x.Value.ToDateTime(TimeOnly.MinValue) : null);
				mc.CreateMap<DateTime?, DateOnly?>().ConvertUsing(x => x != null ? DateOnly.FromDateTime(x.Value) : null);
			});

			services.AddSingleton(mappingConfig.CreateMapper());

			services.Configure<TerminiApiAppSettings>(configuration.GetSection(nameof(TerminiApiAppSettings)));
		}
	}
}
