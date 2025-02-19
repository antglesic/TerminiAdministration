using TerminiService.WeatherService.Models;

namespace TerminiService.WeatherService
{
	public interface IWeatherService
	{
		Task<GetWeatherForecastResponse> GetWeatherForecast(GetWeatherForecastRequest request);
	}
}
