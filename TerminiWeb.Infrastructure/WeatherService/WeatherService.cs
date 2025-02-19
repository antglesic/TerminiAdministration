using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Serilog;
using TerminiWeb.Infrastructure.Common.Client;
using TerminiWeb.Infrastructure.Common.Configuration;
using TerminiWeb.Infrastructure.WeatherService.Dtos;
using TerminiWeb.Infrastructure.WeatherService.Models;

namespace TerminiWeb.Infrastructure.WeatherService
{
	public class WeatherService : ExternalServiceBase, IWeatherService
	{
		#region Constants

		private const string _controllerEndpoint = "WeatherForecast";

		#endregion

		#region Fields

		private readonly ApiEndpointSettings _apiEndpointSettings;
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _jsonSerializerOptions;
		private readonly ILogger _logger = Log.ForContext<WeatherService>();

		#endregion

		#region Constructor

		public WeatherService(HttpClient httpClient, IOptions<ApiEndpointSettings> apiEndpointSettings) : base(apiEndpointSettings)
		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_apiEndpointSettings = apiEndpointSettings.Value;
			_jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
		}

		#endregion

		#region Methods

		public async Task<GetWeatherForecastResponse> GetWeatherForecast(GetWeatherForecastRequest request)
		{
			GetWeatherForecastResponse response = new GetWeatherForecastResponse()
			{
				Request = request
			};

			if (request != null)
			{
				try
				{
					string Apiurl = $"{_apiEndpointSettings.TerminiApiBaseUrl}/{_controllerEndpoint}/GetWeatherForecasts";
					_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(@"Bearer", ExternalServicesHelper.CreateToken());

					using (HttpResponseMessage responseContent = await _httpClient.GetAsync(Apiurl))
					{
						if (responseContent.IsSuccessStatusCode)
						{
							Stream responseStream = await responseContent.Content.ReadAsStreamAsync();

							if (responseStream != null && responseStream.Length > 0)
							{
								WeatherForecastResponseDto retval = await JsonSerializer.DeserializeAsync<WeatherForecastResponseDto>(responseStream, _jsonSerializerOptions);

								if (retval != null && retval.WeatherForecasts != null && retval.WeatherForecasts.Any())
								{
									response.WeatherForecastResponse = retval;
									response.Success = true;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					_logger
						.ForContext("GetWeatherForecast", request.RequestToken, true)
						.Error(ex, ex.Message);
					throw;
				}
			}

			return response;
		}

		#endregion
	}
}
