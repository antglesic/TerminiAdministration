using Serilog;
using TerminiService.WeatherService.Dtos;
using TerminiService.WeatherService.Models;

namespace TerminiService.WeatherService
{
	public class WeatherService : IWeatherService
	{
		#region StaticConstants

		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		#endregion

		#region Fields

		private readonly ILogger _logger = Log.ForContext<WeatherService>();

		#endregion

		#region Constructor

		public WeatherService()
		{
		}

		#endregion

		#region Methods

		public async Task<GetWeatherForecastResponse> GetWeatherForecast(GetWeatherForecastRequest request)
		{
			GetWeatherForecastResponse response = new GetWeatherForecastResponse()
			{
				Request = request
			};

			try
			{
				await Task.Delay(100);

				IEnumerable<WeatherForecastDto> weatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecastDto
				{
					Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
					TemperatureC = Random.Shared.Next(-20, 55),
					Summary = Summaries[Random.Shared.Next(Summaries.Length)]
				})
				.ToArray();

				if (weatherForecasts != null && weatherForecasts.Any())
				{
					WeatherForcastResponseDto weatherForcastResponseDto = new WeatherForcastResponseDto();
					weatherForcastResponseDto.WeatherForecasts = weatherForecasts;

					response.WeatherForecastResponse = weatherForcastResponseDto;
					response.Success = true;
				}
			}
			catch (Exception ex)
			{
				_logger
						.ForContext("GetWeatherForecast", request.RequestToken, true)
						.Error(ex, ex.Message);
				throw;
			}

			return response;
		}

		#endregion
	}
}
