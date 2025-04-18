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

		public async Task<CreateTerminResponse> CreateTermin(CreateTerminRequest request)
		{
			CreateTerminResponse response = new CreateTerminResponse()
			{
				Request = request
			};

			try
			{
				string apiUrl = $"{_apiEndpointSettings.TerminiApiBaseUrl}/{_controllerEndpoint}/CreateTermin";
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ExternalServicesHelper.CreateToken());

				var content = JsonSerializer.Serialize(request.CreateTermin, _jsonSerializerOptions);
				var bodyContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json");

				using (HttpResponseMessage responseContent = await _httpClient.PostAsync(apiUrl, bodyContent))
				{
					if (responseContent.IsSuccessStatusCode)
					{
						Stream responseStream = await responseContent.Content.ReadAsStreamAsync();

						if (responseStream != null && responseStream.Length > 0)
						{
							TerminDto? createdTermin = await JsonSerializer.DeserializeAsync<TerminDto>(responseStream, _jsonSerializerOptions);

							if (createdTermin != null)
							{
								response.Termin = createdTermin;
								response.Success = true;
							}
							else
							{
								response.Message = "Failed to deserialize created termin";
							}
						}
					}
					else
					{
						response.Message = $"Failed to create termin. Status: {responseContent.StatusCode}";
					}
				}
			}
			catch (Exception ex)
			{
				_logger
					.ForContext("CreateTermin", request.RequestToken, true)
					.Error(ex, ex.Message);
				throw;
			}

			return response;
		}

		public async Task<SetPlayerRatingsResponse> SetPlayerRatings(SetPlayerRatingsRequest request)
		{
			SetPlayerRatingsResponse response = new SetPlayerRatingsResponse()
			{
				Request = request
			};

			try
			{
				string apiUrl = $"{_apiEndpointSettings.TerminiApiBaseUrl}/{_controllerEndpoint}/SetPlayerRatings";
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ExternalServicesHelper.CreateToken());

				var content = JsonSerializer.Serialize(request.TerminPlayers, _jsonSerializerOptions);
				var bodyContent = new StringContent(content, System.Text.Encoding.UTF8, "application/json");

				using (HttpResponseMessage responseContent = await _httpClient.PostAsync(apiUrl, bodyContent))
				{
					if (responseContent.IsSuccessStatusCode)
					{
						response.Success = true;
					}
					else
					{
						response.Message = $"Failed to set player ratings. Status: {responseContent.StatusCode}";
					}
				}
			}
			catch (Exception ex)
			{
				_logger
					.ForContext("SetPlayerRatings", request.RequestToken, true)
					.Error(ex, ex.Message);
				throw;
			}

			return response;
		}

		#endregion

	}
}
