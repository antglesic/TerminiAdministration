using Microsoft.AspNetCore.Components;
using TerminiWeb.Infrastructure.WeatherService;
using TerminiWeb.Infrastructure.WeatherService.Dtos;
using TerminiWeb.Infrastructure.WeatherService.Models;

namespace TerminiWeb.Pages
{
	public partial class Weather : ComponentBase
	{
		#region Injections

		[Inject]
		WeatherService _forecastService { get; set; }

		#endregion

		#region Fields

		private WeatherForecastDto[]? _forecasts;
		private bool firstLoad = true;

		#endregion

		#region Methods

		protected override async Task OnInitializedAsync()
		{
			if (firstLoad)
			{
				firstLoad = false;
				bool wasLoaded = await GetForecasts();
				StateHasChanged();
			}

			await base.OnInitializedAsync();
		}

		private async Task<bool> GetForecasts()
		{
			GetWeatherForecastRequest request = new GetWeatherForecastRequest();

			GetWeatherForecastResponse response = await _forecastService.GetWeatherForecast(request);

			if (response == null)
			{
				return false;
			}
			else
			{
				if (response.WeatherForecastResponse != null
					&& response.WeatherForecastResponse.WeatherForecasts != null
					&& response.WeatherForecastResponse.WeatherForecasts.Any())
				{
					_forecasts = response.WeatherForecastResponse.WeatherForecasts.ToArray();
				}
			}

			return true;
		}

		#endregion
	}
}
