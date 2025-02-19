using TerminiWeb.Infrastructure.WeatherService.Models;

namespace TerminiWeb.Infrastructure.WeatherService
{
	public interface IWeatherService
	{
		Task<GetWeatherForecastResponse> GetWeatherForecast(GetWeatherForecastRequest request);
	}
}
