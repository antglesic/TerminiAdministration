using TerminiDomain.Core;
using TerminiService.WeatherService.Dtos;

namespace TerminiService.WeatherService.Models
{
	public class GetWeatherForecastRequest : RequestBase
	{
	}

	public class GetWeatherForecastResponse : ResponseBase<GetWeatherForecastRequest>
	{
		public WeatherForcastResponseDto WeatherForecastResponse { get; set; }
	}
}
