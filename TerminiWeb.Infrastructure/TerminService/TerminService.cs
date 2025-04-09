using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Serilog;
using TerminiWeb.Infrastructure.Common.Client;
using TerminiWeb.Infrastructure.Common.Configuration;
using TerminiWeb.Infrastructure.TerminService.Dtos;
using TerminiWeb.Infrastructure.TerminService.Models;

namespace TerminiWeb.Infrastructure.TerminService
{
	public class TerminService : ExternalServiceBase, ITerminService
	{
		#region Constants

		private string _controllerEndpoint = "Termin";

		#endregion

		#region Fields

		private readonly ApiEndpointSettings _apiEndpointSettings;
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _jsonSerializerOptions;
		private readonly ILogger _logger = Log.ForContext<TerminService>();

		#endregion

		#region Constructor

		public TerminService(HttpClient httpClient, IOptions<ApiEndpointSettings> apiEndpointSettings) : base(apiEndpointSettings)
		{
			_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			_apiEndpointSettings = apiEndpointSettings.Value;
			_jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
		}

		#endregion

		#region Methods

		public async Task<GetTerminsResponse> GetTermins(GetTerminsRequest request)
		{
			GetTerminsResponse response = new GetTerminsResponse()
			{
				Request = request
			};

			if (request != null)
			{
				try
				{
					string Apiurl = $"{_apiEndpointSettings.TerminiApiBaseUrl}/{_controllerEndpoint}/GetTermins";
					_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(@"Bearer", ExternalServicesHelper.CreateToken());

					using (HttpResponseMessage responseContent = await _httpClient.GetAsync(Apiurl))
					{
						if (responseContent.IsSuccessStatusCode)
						{
							Stream responseStream = await responseContent.Content.ReadAsStreamAsync();

							if (responseStream != null && responseStream.Length > 0)
							{
								TerminListResponseDto? terminList = await JsonSerializer.DeserializeAsync<TerminListResponseDto>(responseStream, _jsonSerializerOptions);

								if (terminList != null && terminList.Termins != null && terminList.Termins.Any())
								{
									response.Termins = terminList.Termins;
									response.Success = true;
								}
								else
								{
									response.Message = "No termins were found!";
								}
							}
						}
						else
						{
							response.Message = "Failed to get termin list";
						}
					}
				}
				catch (Exception ex)
				{
					_logger
						.ForContext("GetTermins", request.RequestToken, true)
						.Error(ex, ex.Message);
					throw;
				}
			}
			else
			{
				response.Message = "Request is null";
			}

			return response;
		}

		#endregion

	}
}
