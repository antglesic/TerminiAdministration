using TerminiDomain.Core;
using TerminiWeb.Infrastructure.WeatherService.Dtos;

namespace TerminiWeb.Infrastructure.WeatherService.Models
{
	public class GetWeatherForecastRequest : RequestBase
	{
	}

	public class GetWeatherForecastResponse : ResponseBase<GetWeatherForecastRequest>
	{
		public WeatherForecastResponseDto WeatherForecastResponse { get; set; }
	}
}

