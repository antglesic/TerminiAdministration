namespace TerminiWeb.Infrastructure.WeatherService.Dtos
{
	public class WeatherForecastResponseDto
	{
		public IEnumerable<WeatherForecastDto> WeatherForecasts { get; set; }
	}
}
