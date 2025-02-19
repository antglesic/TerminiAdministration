using Microsoft.AspNetCore.Mvc;
using TerminiService.WeatherService;
using TerminiService.WeatherService.Dtos;
using TerminiService.WeatherService.Models;

namespace TerminiAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
	#region Fields

	private readonly IWeatherService _weatherService;
	private readonly ILogger<WeatherForecastController> _logger;

	#endregion

	public WeatherForecastController(IWeatherService weatherService, ILogger<WeatherForecastController> logger)
	{
		_weatherService = weatherService;
		_logger = logger;
	}

	[HttpGet("GetWeatherForecasts")]
	public async Task<ActionResult<WeatherForcastResponseDto>> GetWeatherForecasts()
	{
		GetWeatherForecastRequest request = new GetWeatherForecastRequest();

		GetWeatherForecastResponse response = await _weatherService.GetWeatherForecast(request);

		if (!response.Success)
			return BadRequest(response.Message);

		return Ok(response.WeatherForecastResponse);
	}
}
