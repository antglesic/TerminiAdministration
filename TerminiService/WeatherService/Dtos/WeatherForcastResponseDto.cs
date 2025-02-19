namespace TerminiService.WeatherService.Dtos
{
	public class WeatherForcastResponseDto
	{
		public IEnumerable<WeatherForecastDto> WeatherForecasts { get; set; }
	}
}
